/*!
 * AmbModal v1.0 | MIT License
 * Attribute-driven modal framework — Bootstrap 5.3 API compatible
 */
(function (root, factory) {
  if (typeof module === 'object' && module.exports) module.exports = factory();
  else root.AmbModal = factory();
})(typeof globalThis !== 'undefined' ? globalThis : this, function () {
  'use strict';

  /* ── Helpers ──────────────────────────────────── */
  const $ = (sel, ctx = document) => ctx.querySelector(sel);
  const $$ = (sel, ctx = document) => [...ctx.querySelectorAll(sel)];
  const attr = (el, name) => el.getAttribute(name);
  const hasAttr = (el, name) => el.hasAttribute(name);
  const uid = () => 'amb-' + Math.random().toString(36).slice(2, 8);
  const clamp = (v, min, max) => Math.min(Math.max(v, min), max);

  /** Merge objects (shallow) */
  const merge = (...objs) => Object.assign({}, ...objs);

  /** Dispatch custom event, returns false if cancelled */
  const emit = (el, name, detail = {}) => {
    const e = new CustomEvent(name + '.amb.modal', { bubbles: true, cancelable: true, detail });
    return el.dispatchEvent(e);
  };

  /** Run fn after CSS animation/transition ends (or immediately if none) */
  const afterAnim = (el, fn) => {
    const dur = parseFloat(getComputedStyle(el).animationDuration) * 1000 || 0;
    if (dur > 0) el.addEventListener('animationend', fn, { once: true });
    else fn();
  };

  /** Focus first focusable element inside el (skip close btn) */
  const focusFirst = (el) => {
    const sel = 'input:not([disabled]),select:not([disabled]),textarea:not([disabled]),' +
      'button:not([disabled]):not(.amb-close),[tabindex]:not([tabindex="-1"]),a[href]';
    const first = $(sel, el);
    if (first) first.focus();
    else el.focus();
  };

  /** Trap focus inside el */
  const trapFocus = (el, e) => {
    const focusable = $$('button:not([disabled]),input:not([disabled]),select:not([disabled]),' +
      'textarea:not([disabled]),[tabindex]:not([tabindex="-1"]),a[href]', el)
      .filter(n => n.offsetParent !== null);
    if (!focusable.length) return;
    const first = focusable[0], last = focusable[focusable.length - 1];
    if (e.shiftKey ? document.activeElement === first : document.activeElement === last) {
      e.preventDefault();
      (e.shiftKey ? last : first).focus();
    }
  };

  /** Apply CSS variable overrides to wrapper */
  const applyVars = (wrapper, cfg) => {
    if (cfg.duration) wrapper.style.setProperty('--amb-duration', cfg.duration + 'ms');
    if (cfg.easing)   wrapper.style.setProperty('--amb-easing', cfg.easing);
    if (cfg.backdropColor) {
      const bd = $('.amb-backdrop', wrapper.parentNode) || wrapper.previousElementSibling;
      if (bd) bd.style.background = cfg.backdropColor;
    }
  };

  /* ── Z-index stack ────────────────────────────── */
  let _z = 1050;
  const nextZ = () => (_z += 10);
  const resetZ = () => { _z = 1050; };

  /* ── Instance registry ────────────────────────── */
  const _registry = new Map(); // id → Modal instance

  /* ── Default config ───────────────────────────── */
  const DEFAULTS = {
    title: '',
    body: '',
    footer: '',
    size: 'md',
    position: 'center',
    animOpen: 'zoom-in',
    animClose: 'zoom-out',
    duration: 300,
    easing: 'ease-in-out',
    backdrop: true,       // true | 'static' | false
    backdropColor: '',
    keyboard: true,
    autoFocus: true,
    autoClose: 0,
    draggable: false,
    rtl: false,
    theme: '',            // '' | 'dark'
    width: '',
    mobileBreakpoint: 576,
    mobileSize: '',
    noDefaultStyle: false,
    // callbacks
    onload: null,
    afterload: null,
    onclose: null,
    afterclose: null,
    onbackdropclick: null,
  };

  /* ── SVG close icon ───────────────────────────── */
  const CLOSE_SVG = `<svg width="14" height="14" viewBox="0 0 14 14" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round">
    <line x1="1" y1="1" x2="13" y2="13"/><line x1="13" y1="1" x2="1" y2="13"/>
  </svg>`;

  /* ── Build DOM ────────────────────────────────── */
  const buildDOM = (id, cfg) => {
    const pos = cfg.position.replace(/\s+/g, '-');
    const sizeClass = cfg.size === 'custom' ? '' : 'amb-' + cfg.size;
    const themeClass = cfg.theme === 'dark' ? 'amb-dark' : '';
    const rtlClass = cfg.rtl ? 'amb-rtl' : '';

    // backdrop
    const backdrop = cfg.backdrop !== false ? (() => {
      const bd = document.createElement('div');
      bd.className = 'amb-backdrop' + (themeClass ? ' ' + themeClass : '');
      return bd;
    })() : null;

    // wrapper
    const wrapper = document.createElement('div');
    wrapper.className = ['amb-wrapper', 'amb-pos-' + pos, themeClass, rtlClass].filter(Boolean).join(' ');
    wrapper.setAttribute('role', 'dialog');
    wrapper.setAttribute('aria-modal', 'true');
    wrapper.setAttribute('aria-labelledby', id + '-title');
    wrapper.setAttribute('aria-describedby', id + '-body');
    wrapper.id = id + '-wrapper';
    wrapper.tabIndex = -1;

    // dialog
    const dialog = document.createElement('div');
    dialog.className = ['amb-dialog', sizeClass, cfg.draggable ? 'amb-draggable' : ''].filter(Boolean).join(' ');
    dialog.tabIndex = -1;
    if (cfg.width) dialog.style.maxWidth = cfg.width;

    // header
    const header = document.createElement('div');
    header.className = 'amb-header' + (cfg.rtl ? ' amb-header-rtl' : '');

    const title = document.createElement('h5');
    title.className = 'amb-title';
    title.id = id + '-title';
    title.innerHTML = cfg.title;

    const closeBtn = document.createElement('button');
    closeBtn.className = 'amb-close';
    closeBtn.setAttribute('aria-label', 'Close');
    closeBtn.innerHTML = CLOSE_SVG;
    closeBtn.dataset.ambDismiss = 'modal';

    header.append(title, closeBtn);

    // body
    const body = document.createElement('div');
    body.className = 'amb-body';
    body.id = id + '-body';
    body.innerHTML = cfg.body;

    dialog.append(header, body);

    // footer
    if (cfg.footer) {
      const footer = document.createElement('div');
      footer.className = 'amb-footer' + (cfg.rtl ? ' amb-footer-rtl' : '');
      footer.innerHTML = cfg.footer;
      dialog.append(footer);
    }

    wrapper.append(dialog);

    return { backdrop, wrapper, dialog, body };
  };

  /* ── Draggable logic ──────────────────────────── */
  const makeDraggable = (dialog) => {
    const header = $('.amb-header', dialog);
    if (!header) return;
    let ox = 0, oy = 0, dx = 0, dy = 0;
    const onMove = (e) => {
      const cx = e.touches ? e.touches[0].clientX : e.clientX;
      const cy = e.touches ? e.touches[0].clientY : e.clientY;
      dx = cx - ox; dy = cy - oy;
      dialog.style.transform = `translate(${dx}px,${dy}px)`;
    };
    const onUp = () => {
      document.removeEventListener('mousemove', onMove);
      document.removeEventListener('mouseup', onUp);
      document.removeEventListener('touchmove', onMove);
      document.removeEventListener('touchend', onUp);
    };
    header.addEventListener('mousedown', (e) => {
      if (e.target.closest('.amb-close')) return;
      ox = e.clientX - dx; oy = e.clientY - dy;
      document.addEventListener('mousemove', onMove);
      document.addEventListener('mouseup', onUp);
    });
    header.addEventListener('touchstart', (e) => {
      if (e.target.closest('.amb-close')) return;
      ox = e.touches[0].clientX - dx; oy = e.touches[0].clientY - dy;
      document.addEventListener('touchmove', onMove, { passive: true });
      document.addEventListener('touchend', onUp);
    }, { passive: true });
  };

  /* ══════════════════════════════════════════════
     Modal Class
  ══════════════════════════════════════════════ */
  class Modal {
    /**
     * @param {string|HTMLElement} target - modal id or element
     * @param {object} [options]
     */
    constructor(target, options = {}) {
      // Accept element or id string
      if (typeof target === 'string' && !target.startsWith('#')) {
        this._id = target;
        this._el = document.getElementById(target);
      } else if (typeof target === 'string') {
        this._id = target.slice(1);
        this._el = document.getElementById(this._id);
      } else {
        this._el = target;
        this._id = target.id || uid();
        if (!target.id) target.id = this._id;
      }

      // Parse data-amb-* attributes from source element
      const src = this._el;
      const fromAttr = src ? {
        title:            attr(src, 'data-amb-title') || '',
        body:             src.innerHTML || '',
        size:             attr(src, 'data-amb-size') || undefined,
        position:         attr(src, 'data-amb-position') || undefined,
        animOpen:         attr(src, 'data-amb-anim-open') || undefined,
        animClose:        attr(src, 'data-amb-anim-close') || undefined,
        duration:         attr(src, 'data-amb-duration') ? +attr(src, 'data-amb-duration') : undefined,
        easing:           attr(src, 'data-amb-easing') || undefined,
        backdrop:         hasAttr(src, 'data-amb-backdrop') ? attr(src, 'data-amb-backdrop') === 'false' ? false : attr(src, 'data-amb-backdrop') || true : undefined,
        backdropColor:    attr(src, 'data-amb-backdrop-color') || undefined,
        keyboard:         hasAttr(src, 'data-amb-keyboard') ? attr(src, 'data-amb-keyboard') !== 'false' : undefined,
        autoFocus:        hasAttr(src, 'data-amb-auto-focus') ? attr(src, 'data-amb-auto-focus') !== 'false' : undefined,
        autoClose:        attr(src, 'data-amb-auto-close') ? +attr(src, 'data-amb-auto-close') : undefined,
        draggable:        attr(src, 'data-amb-draggable') === 'true',
        rtl:              attr(src, 'data-amb-rtl') === 'true',
        theme:            attr(src, 'data-amb-theme') || undefined,
        width:            attr(src, 'data-amb-width') || undefined,
        mobileBreakpoint: attr(src, 'data-amb-mobile-breakpoint') ? +attr(src, 'data-amb-mobile-breakpoint') : undefined,
        mobileSize:       attr(src, 'data-amb-mobile-size') || undefined,
        noDefaultStyle:   hasAttr(src, 'data-amb-no-default-style'),
      } : {};

      // Remove undefined keys
      Object.keys(fromAttr).forEach(k => fromAttr[k] === undefined && delete fromAttr[k]);

      this.cfg = merge(DEFAULTS, fromAttr, options);
      this._visible = false;
      this._autoCloseTimer = null;
      this._trapHandler = null;
      this._keyHandler = null;

      this._build();
      _registry.set(this._id, this);
    }

    /** Build and inject DOM */
    _build() {
      const { backdrop, wrapper, dialog, body } = buildDOM(this._id, this.cfg);
      this._backdrop = backdrop;
      this._wrapper = wrapper;
      this._dialog = dialog;
      this._body = body;

      const z = nextZ();
      if (backdrop) { backdrop.style.zIndex = z; backdrop.style.display = 'none'; document.body.append(backdrop); }
      wrapper.style.zIndex = z + 1;
      wrapper.style.display = 'none';
      document.body.append(wrapper);

      if (this.cfg.draggable) makeDraggable(dialog);

      // Mobile size override
      this._applyMobileSize();
      window.addEventListener('resize', () => this._applyMobileSize());

      // Dismiss buttons + backdrop click (wrapper covers full viewport)
      wrapper.addEventListener('click', (e) => {
        if (e.target.closest('[data-amb-dismiss="modal"]')) { this.hide(); return; }
        if (e.target === wrapper) {
          emit(wrapper, 'backdropclick', { id: this._id });
          if (typeof this.cfg.onbackdropclick === 'function') this.cfg.onbackdropclick(this);
          if (this.cfg.backdrop === true) this.hide();
        }
      });
    }

    _applyMobileSize() {
      const bp = this.cfg.mobileBreakpoint;
      const ms = this.cfg.mobileSize;
      if (!ms) return;
      const isMobile = window.innerWidth <= bp;
      const sizes = ['amb-sm','amb-md','amb-lg','amb-xl','amb-xxl','amb-full'];
      sizes.forEach(s => this._dialog.classList.remove(s));
      this._dialog.classList.add('amb-' + (isMobile ? ms : this.cfg.size));
    }

    /** Show the modal */
    show() {
      if (this._visible) return this;
      const { cfg, _wrapper: w, _backdrop: bd, _dialog: d } = this;

      // onload callback / event
      if (typeof cfg.onload === 'function') cfg.onload(this);
      if (!emit(w, 'show', { id: this._id })) return this;

      this._visible = true;
      const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
      if (scrollbarWidth > 0) document.body.style.paddingRight = scrollbarWidth + 'px';
      document.body.style.overflow = 'hidden';

      if (bd) { bd.style.display = ''; requestAnimationFrame(() => bd.classList.add('amb-show')); }
      w.style.display = '';
      requestAnimationFrame(() => w.classList.add('amb-show'));

      // Open animation
      if (cfg.animOpen && cfg.animOpen !== 'none') {
        applyVars(w, cfg);
        d.classList.add('amb-anim-' + cfg.animOpen);
        afterAnim(d, () => {
          d.classList.remove('amb-anim-' + cfg.animOpen);
          if (typeof cfg.afterload === 'function') cfg.afterload(this);
          emit(w, 'shown', { id: this._id });
        });
      } else {
        if (typeof cfg.afterload === 'function') cfg.afterload(this);
        emit(w, 'shown', { id: this._id });
      }

      // Focus
      if (cfg.autoFocus) setTimeout(() => focusFirst(d), 50);

      // Focus trap
      this._trapHandler = (e) => { if (e.key === 'Tab') trapFocus(d, e); };
      document.addEventListener('keydown', this._trapHandler);

      // ESC key
      if (cfg.keyboard) {
        this._keyHandler = (e) => { if (e.key === 'Escape') this.hide(); };
        document.addEventListener('keydown', this._keyHandler);
      }

      // Auto close
      if (cfg.autoClose > 0) {
        this._autoCloseTimer = setTimeout(() => this.hide(), cfg.autoClose);
      }

      return this;
    }

    /** Hide the modal */
    hide() {
      if (!this._visible) return this;
      const { cfg, _wrapper: w, _backdrop: bd, _dialog: d } = this;

      if (typeof cfg.onclose === 'function' && cfg.onclose(this) === false) return this;
      if (!emit(w, 'hide', { id: this._id })) return this;

      this._visible = false;
      clearTimeout(this._autoCloseTimer);
      document.removeEventListener('keydown', this._trapHandler);
      document.removeEventListener('keydown', this._keyHandler);

      const done = () => {
        w.classList.remove('amb-show');
        w.style.display = 'none';
        if (bd) { bd.classList.remove('amb-show'); bd.style.display = 'none'; }
        // Restore body scroll only if no other modals open
        if (![..._registry.values()].some(m => m._visible)) {
          document.body.style.overflow = '';
          document.body.style.paddingRight = '';
        }
        if (typeof cfg.afterclose === 'function') cfg.afterclose(this);
        emit(w, 'hidden', { id: this._id });
        // Restore focus
        if (this._triggerEl) { this._triggerEl.focus(); this._triggerEl = null; }
      };

      if (cfg.animClose && cfg.animClose !== 'none') {
        applyVars(w, cfg);
        d.classList.add('amb-anim-close-' + cfg.animClose);
        if (bd) bd.classList.remove('amb-show');
        afterAnim(d, () => { d.classList.remove('amb-anim-close-' + cfg.animClose); done(); });
      } else {
        done();
      }

      return this;
    }

    /** Toggle visibility */
    toggle() { return this._visible ? this.hide() : this.show(); }

    /** Update config and rebuild */
    config(options) {
      this.cfg = merge(this.cfg, options);
      return this;
    }

    /** Destroy instance and remove DOM */
    dispose() {
      this.hide();
      setTimeout(() => {
        this._wrapper.remove();
        if (this._backdrop) this._backdrop.remove();
        _registry.delete(this._id);
      }, this.cfg.duration + 50);
    }

    /** Set loading overlay */
    setLoading(state) {
      let overlay = $('.amb-loading', this._dialog);
      if (state) {
        if (!overlay) {
          overlay = document.createElement('div');
          overlay.className = 'amb-loading';
          overlay.innerHTML = '<div class="amb-spinner"></div>';
          this._dialog.append(overlay);
        }
      } else {
        if (overlay) overlay.remove();
      }
      return this;
    }

    get isVisible() { return this._visible; }
    get element() { return this._wrapper; }
    get id() { return this._id; }

    /** Bootstrap 5.3 compatible static getInstance */
    static getInstance(el) {
      const id = typeof el === 'string' ? el : (el && el.id);
      return id ? _registry.get(id) || null : null;
    }

    /** Bootstrap 5.3 compatible static getOrCreateInstance */
    static getOrCreateInstance(el, options) {
      return Modal.getInstance(el) || new Modal(el, options);
    }
  }

  /* ══════════════════════════════════════════════
     AmbModal public API
  ══════════════════════════════════════════════ */

  /**
   * Create a modal programmatically
   * @param {string} id
   * @param {object} options
   * @returns {Modal}
   */
  const create = (id, options = {}) => {
    if (_registry.has(id)) return _registry.get(id);
    // Create a ghost source element
    const src = document.createElement('div');
    src.id = id;
    src.style.display = 'none';
    document.body.append(src);
    return new Modal(id, options);
  };

  /**
   * Open a modal by id
   * @param {string} id
   * @param {object} [options]
   */
  const open = (id, options) => {
    const m = _registry.get(id) || create(id, options);
    if (options) m.config(options);
    if (!m._visible) m.show();
    return m;
  };

  /**
   * Close a modal by id
   * @param {string} id
   */
  const close = (id) => { const m = _registry.get(id); if (m) m.hide(); };

  /**
   * Destroy a modal by id
   * @param {string} id
   */
  const destroy = (id) => { const m = _registry.get(id); if (m) m.dispose(); };

  /**
   * Global config defaults
   * @param {object} options
   */
  const config = (options) => Object.assign(DEFAULTS, options);

  /* ── Helpers: confirm / alert / setLoading ──── */

  /**
   * Show a confirm dialog
   * @param {object} opts - { message, title, confirmText, cancelText, onConfirm, onCancel, theme }
   */
  const confirm = (opts = {}) => {
    const id = uid();
    const confirmText = opts.confirmText || 'Confirm';
    const cancelText  = opts.cancelText  || 'Cancel';
    const m = create(id, {
      title: opts.title || 'Confirm',
      body: `<div class="amb-confirm-body">
        <p class="amb-confirm-msg">${opts.message || ''}</p>
        <div class="amb-confirm-btns">
          <button class="amb-btn amb-btn-outline" data-amb-action="cancel">${cancelText}</button>
          <button class="amb-btn amb-btn-primary" data-amb-action="confirm">${confirmText}</button>
        </div>
      </div>`,
      size: opts.size || 'sm',
      animOpen: opts.animOpen || 'zoom-in',
      animClose: opts.animClose || 'zoom-out',
      theme: opts.theme || '',
      rtl: opts.rtl || false,
      backdrop: 'static',
      keyboard: false,
    });

    m._wrapper.addEventListener('click', (e) => {
      const action = e.target.closest('[data-amb-action]');
      if (!action) return;
      if (action.dataset.ambAction === 'confirm') {
        if (typeof opts.onConfirm === 'function') opts.onConfirm();
      } else {
        if (typeof opts.onCancel === 'function') opts.onCancel();
      }
      m.hide();
      setTimeout(() => m.dispose(), 400);
    });

    m.show();
    return m;
  };

  /** Alert type icons */
  const ALERT_ICONS = {
    success: `<svg viewBox="0 0 24 24" width="28" height="28" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>`,
    error:   `<svg viewBox="0 0 24 24" width="28" height="28" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>`,
    warning: `<svg viewBox="0 0 24 24" width="28" height="28" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/><line x1="12" y1="9" x2="12" y2="13"/><line x1="12" y1="17" x2="12.01" y2="17"/></svg>`,
    info:    `<svg viewBox="0 0 24 24" width="28" height="28" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><line x1="12" y1="16" x2="12" y2="12"/><line x1="12" y1="8" x2="12.01" y2="8"/></svg>`,
  };

  /**
   * Show an alert modal
   * @param {object} opts - { message, type, title, btnText, theme, autoClose }
   */
  const alert = (opts = {}) => {
    const id = uid();
    const type = opts.type || 'info';
    const icon = ALERT_ICONS[type] || ALERT_ICONS.info;
    const btnText = opts.btnText || 'OK';
    const m = create(id, {
      title: opts.title || type.charAt(0).toUpperCase() + type.slice(1),
      body: `<div class="amb-alert-body amb-alert-${type}">
        <div class="amb-alert-icon">${icon}</div>
        <p class="amb-alert-msg">${opts.message || ''}</p>
      </div>`,
      footer: `<button class="amb-btn amb-btn-${type === 'error' ? 'danger' : type === 'warning' ? 'warning' : type === 'success' ? 'success' : 'info'}" data-amb-dismiss="modal">${btnText}</button>`,
      size: opts.size || 'sm',
      animOpen: opts.animOpen || 'bounce',
      animClose: opts.animClose || 'zoom-out',
      theme: opts.theme || '',
      rtl: opts.rtl || false,
      autoClose: opts.autoClose || 0,
      backdrop: 'static',
    });

    m._wrapper.addEventListener('hidden.amb.modal', () => setTimeout(() => m.dispose(), 100), { once: true });
    m.show();
    return m;
  };

  /**
   * Set loading state on a modal
   * @param {string} id
   * @param {boolean} state
   */
  const setLoading = (id, state) => {
    const m = _registry.get(id);
    if (m) m.setLoading(state);
  };

  /* ── Attribute-driven init (data-amb-toggle) ── */
  const initFromDOM = () => {
    // Toggle buttons: data-amb-toggle="modal" data-amb-target="#myModal"
    document.addEventListener('click', (e) => {
      const btn = e.target.closest('[data-amb-toggle="modal"]');
      if (!btn) return;
      const targetSel = attr(btn, 'data-amb-target') || attr(btn, 'data-amb-id');
      if (!targetSel) return;
      const id = targetSel.replace('#', '');
      const srcEl = document.getElementById(id);
      let m = _registry.get(id);
      if (!m && srcEl) m = new Modal(srcEl);
      if (!m) return;
      m._triggerEl = btn;
      m.toggle();
    });

    // Auto-init modals with data-amb="modal"
    $$('[data-amb="modal"]').forEach(el => {
      if (!el.id) el.id = uid();
      if (!_registry.has(el.id)) new Modal(el);
    });
  };

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initFromDOM);
  } else {
    initFromDOM();
  }

  /* ── Public API ───────────────────────────────── */
  return { Modal, create, open, close, destroy, config, confirm, alert, setLoading };
});
