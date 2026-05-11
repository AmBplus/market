/**
 * PartialModalManager — AmbModal Edition
 * -------------------------------------------------------------
 * Drop-in replacement for partialmodalmanager.js using AmbModal
 * instead of Bootstrap. All public APIs are identical.
 *
 * Requires: ambmodal.js (global AmbModal)
 * Optional: jQuery (only for $.validator unobtrusive + DataTable)
 */
(function () {
    'use strict';

    /************************************************************
     MODAL ADAPTER
     -------------------------------------------------------------
     Swap modal library behavior here only.
     Event names follow AmbModal convention: action.amb.modal
    ************************************************************/

    const ModalAdapter = {

        // Event fired when modal is about to show
        showEvent: 'show.amb.modal',

        // Event fired after modal is fully hidden
        hideEvent: 'hidden.amb.modal',

        // AmbModal passes the trigger element via _triggerEl on the instance,
        // but the show event detail also carries it for convenience.
        getTriggerButton: function (event) {
            return (event.detail && event.detail.relatedTarget) || null;
        },

        // Hide modal programmatically via AmbModal API
        hide: function (modalId) {
            AmbModal.close(modalId);
        }

    };


    /************************************************************
     MODAL REGISTRY
     Prevents duplicate event binding on the same modal.
    ************************************************************/

    const modalRegistry = {};

    function ensureModalBound(modalId) {
        if (modalRegistry[modalId]) return modalRegistry[modalId];

        // Get or create the AmbModal instance
        let instance = AmbModal.Modal.getInstance(modalId);

        if (!instance) {
            const el = document.getElementById(modalId);
            if (!el) { console.warn('AmbModal: element not found:', modalId); return null; }
            instance = new AmbModal.Modal(el);
        }

        // The wrapper element is where AmbModal fires events
        const wrapperEl = instance._wrapper;

        modalRegistry[modalId] = { instance, wrapperEl, initialized: true };
        return modalRegistry[modalId];
    }


    /************************************************************
     PARTIAL ASSET MANAGER
    ************************************************************/

    let loadedPartialScripts = [];
    let loadedPartialStyles = [];

    function loadPartialScripts(container) {
        container.querySelectorAll('script').forEach(old => {
            const s = document.createElement('script');
            if (old.src) { s.src = old.src; s.async = false; }
            else s.textContent = old.textContent;
            if (old.type) s.type = old.type;
            [...old.attributes].forEach(a => {
                if (a.name !== 'src' && a.name !== 'type') s.setAttribute(a.name, a.value);
            });
            document.body.appendChild(s);
            loadedPartialScripts.push(s);
        });
    }

    function loadPartialStyles(container) {
        container.querySelectorAll('style').forEach(old => {
            const s = document.createElement('style');
            s.textContent = old.textContent;
            [...old.attributes].forEach(a => s.setAttribute(a.name, a.value));
            document.head.appendChild(s);
            loadedPartialStyles.push(s);
        });

        container.querySelectorAll('link[rel="stylesheet"]').forEach(old => {
            const l = document.createElement('link');
            l.rel = 'stylesheet';
            [...old.attributes].forEach(a => { if (a.name !== 'rel') l.setAttribute(a.name, a.value); });
            document.head.appendChild(l);
            loadedPartialStyles.push(l);
        });
    }

    function cleanupPartialAssets() {
        loadedPartialScripts.forEach(s => s.remove());
        loadedPartialStyles.forEach(s => s.remove());
        loadedPartialScripts = [];
        loadedPartialStyles = [];
    }


    /************************************************************
     HELPERS
    ************************************************************/

    function readButtonParams(button) {
        const params = {};
        let handler = '';
        for (const a of button.attributes) {
            if (a.name.startsWith('data-params')) params[a.name.substring(12)] = a.value;
            if (a.name === 'data-pagehandler') handler = a.value;
        }
        return { params, handler };
    }

    function buildHandlerUrl(params, handler) {
        const base = params['api'] || '?handler=' + handler;
        const { api, ...rest } = params;
        const qs = new URLSearchParams(rest).toString();
        return qs ? base + (base.includes('?') ? '&' : '?') + qs : base;
    }

    function loadPartial(modalBody, url) {
        modalBody.innerHTML = '<p>Loading...</p>';
        fetch(url)
            .then(r => r.text())
            .then(html => {
                modalBody.innerHTML = html;
                loadPartialScripts(modalBody);
                loadPartialStyles(modalBody);
                if (typeof $ !== 'undefined' && $.validator)
                    $.validator.unobtrusive.parse(modalBody);
            })
            .catch(() => { modalBody.innerHTML = "<p class='text-danger'>Loading failed.</p>"; });
    }

    function reloadTable(tableId, dataTable) {
        if (tableId && window.SmartDtFireEvent) {
            SmartDtFireEvent(tableId, 'reload');
            return; // SmartDT handled it, no need to go further
        }
        if (dataTable && dataTable.ajax && typeof dataTable.ajax.reload === 'function') {
            dataTable.ajax.reload(null, false);
            return;
        }
        if (tableId && typeof $ !== 'undefined' && $.fn.DataTable && $.fn.DataTable.isDataTable('#' + tableId))
            $('#' + tableId).DataTable().ajax.reload(null, false);
    }

    function showFormError(message) {
        const box = document.getElementById('formErrorBox');
        if (!box) return;
        box.innerHTML = message;
        box.classList.remove('d-none');
        box.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    function clearFormError() {
        const box = document.getElementById('formErrorBox');
        if (!box) return;
        box.classList.add('d-none');
        box.innerHTML = '';
    }

    function handlePostSuccess(message, modalId, dataTable, tableId = null) {
        if (window.Swal) Swal.fire('موفق', message || 'عملیات با موفقیت انجام شد', 'success');
        ModalAdapter.hide(modalId);
        reloadTable(tableId, dataTable);

    }


    /************************************************************
     GET MODAL LOADER
     -------------------------------------------------------------
     Usage (identical to Bootstrap version):
       PartialModalLoaderWithGet('userModal');
    ************************************************************/

    window.PartialModalLoaderWithGet = function (id) {
        const reg = ensureModalBound(id);
        if (!reg) return;

        const { instance, wrapperEl } = reg;

        wrapperEl.addEventListener(ModalAdapter.hideEvent, cleanupPartialAssets);

        // AmbModal fires show.amb.modal on the wrapper; trigger button is
        // stored on instance._triggerEl (set by data-amb-toggle handler).
        wrapperEl.addEventListener(ModalAdapter.showEvent, function () {
            const button = instance._triggerEl;
            if (!button) return;

            const modalBody = wrapperEl.querySelector('.amb-body');
            const { params, handler } = readButtonParams(button);
            loadPartial(modalBody, buildHandlerUrl(params, handler));
        });
    };


    /************************************************************
     POST MODAL LOADER
     -------------------------------------------------------------
     Usage (identical to Bootstrap version):
       PartialModalLoaderWithPost('userModal', 'userForm', 'SaveUser', myDataTable);
    ************************************************************/

    window.PartialModalLoaderWithPost = function (id, formId, handler, dataTable, tableId) {

        const reg = ensureModalBound(id);
        if (!reg) return;

        const { instance, wrapperEl } = reg;

        wrapperEl.addEventListener(ModalAdapter.hideEvent, cleanupPartialAssets);

        wrapperEl.addEventListener(ModalAdapter.showEvent, function () {
            const button = instance._triggerEl;
            if (!button) return;

            const modalBody = wrapperEl.querySelector('.amb-body');
            const { params, handler: handlerName } = readButtonParams(button);
            loadPartial(modalBody, buildHandlerUrl(params, handlerName));

            setTimeout(() => {
                const saveBtn = modalBody.querySelector('.submit');
                if (saveBtn) saveBtn.addEventListener('click', handleFormSubmit);
            }, 200);
        });


        function formDataHasFile(fd) {
            for (const v of fd.values()) if (v instanceof File && v.size > 0) return true;
            return false;
        }

        function handleFormSubmit(e) {
            e.preventDefault();
            const form = document.getElementById(formId);
            if (!form) return;
            clearFormError();
            if (typeof $ !== 'undefined' && $('#' + formId).length && !$('#' + formId).valid()) return;
            const fd = new FormData(form);
            formDataHasFile(fd) ? uploadWithProgress(fd) : submitWithFetch(fd);
        }

        function uploadWithProgress(fd) {
            const xhr = new XMLHttpRequest();
            xhr.open('POST', '?handler=' + handler, true);
            xhr.onload = function () {
                if (window.Swal) Swal.close();
                if (xhr.status >= 200 && xhr.status < 300)
                    handlePostSuccess('عملیات با موفقیت انجام شد', id, dataTable, tableId);

                else
                    showFormError('خطای سرور');
            };
            xhr.send(fd);
        }

        function submitWithFetch(fd) {
            fetch('?handler=' + handler, { method: 'POST', body: fd })
                .then(async res => {
                    if (!res.ok) { const t = await res.text(); showFormError(t); throw new Error(t); }
                    return res.json();
                })
                .then(result => handlePostSuccess(result.messageSingle, id, dataTable, tableId))
                .catch(err => console.error(err));
        }
    };

})();
