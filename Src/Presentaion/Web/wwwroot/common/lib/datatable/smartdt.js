/**
 * SmartDataTable Framework
 * 
 * HTML Attributes:
 *   data-dt-url          - ajax url
 *   data-dt-method       - POST | GET (default: POST)
 *   data-dt-filters      - selector for external filter inputs (e.g. ".user-filters")
 *   data-dt-buttons      - comma separated button keys (e.g. "excelAll,csvCurrent")
 *   data-dt-reload-on    - selector for elements that trigger reload on click
 * 
 *   On <th>:
 *   data-dt-type         - jalali | gregorian | yesno | badge
 *   data-dt-name         - column data key
 *   data-dt-searchable   - true | false
 *   data-dt-orderable    - true | false
 *   data-dt-class        - css class for column
 * 
 *   On filter inputs:
 *   data-dt-target       - table selector this filter belongs to (e.g. "#usersTable")
 *   data-dt-auto-search  - presence enables auto-search on this input
 *   name or data-dt-param - parameter name sent to server
 * 
 *   On reload buttons (outside table):
 *   data-dt-target       - table selector to reload (e.g. "#usersTable")
 */

/**
 * SmartDataTable Framework
 */

(function () {
    'use strict';

    // ───────────────── Registry ─────────────────

    const SmartDtRegistry = {};

    // ───────────────── Column Renderers ─────────────────

    const ColumnTypeRenderers = {
        _registry: {},
        register(type, fn) {
            this._registry[type] = fn;
        },
        get(type) {
            return this._registry[type] || null;
        }
    };

    ColumnTypeRenderers.register('jalali', function (data) {
        if (!data) return '<span>نامشخص</span>';
        const date = new Date(data);
        if (isNaN(date)) return '<span>نامشخص</span>';
        const localized = date.toLocaleDateString('fa-IR', {
            year: 'numeric', month: 'long', day: 'numeric'
        });
        return `<small class="fw-bold text-small">${localized}</small>`;
    });

    ColumnTypeRenderers.register('gregorian', function (data) {
        if (!data) return '<span>-</span>';
        const date = new Date(data);
        if (isNaN(date)) return '<span>-</span>';
        const localized = date.toLocaleDateString('en-GB');
        return `<small class="fw-bold text-small">${localized}</small>`;
    });

    ColumnTypeRenderers.register('yesno', function (data) {
        if (data === true || data === 'true' || data === 1)
            return `<i class="fa fa-check text-success"></i>`;
        return `<i class="fa fa-times text-danger"></i>`;
    });

    ColumnTypeRenderers.register('badge', function (data) {
        if (!data) return '';
        return `<span class="badge bg-secondary">${data}</span>`;
    });

    // ───────────────── Buttons ─────────────────

    const DefaultButtons = {
        _registry: {},
        register(key, factory) {
            this._registry[key] = factory;
        },
        get(key) {
            return this._registry[key] || null;
        }
    };

    DefaultButtons.register('excelAll', dt => ({
        text: '<i class="fa fa-file-excel"></i> Excel (همه)',
        className: 'btn btn-success btn-sm',
        action: (e, d, n, c) => window.exportFunction(e, dt || d, n, c, 1)
    }));

    DefaultButtons.register('excelCurrent', dt => ({
        text: '<i class="fa fa-file-excel"></i> Excel (صفحه)',
        className: 'btn btn-success btn-sm',
        action: (e, d, n, c) => window.exportFunction(e, dt || d, n, c, 2)
    }));

    DefaultButtons.register('csvCurrent', dt => ({
        text: '<i class="fa fa-file-csv"></i> CSV',
        className: 'btn btn-info btn-sm',
        action: (e, d, n, c) => window.exportFunction(e, dt || d, n, c, 3)
    }));

    // ───────────────── SmartDataTable Class ─────────────────

    class SmartDataTable {

        constructor(tableEl, jsOptions = {}) {

            this.$table = $(tableEl);
            this.tableId = tableEl.id || ('sdt_' + Math.random().toString(36).slice(2));
            tableEl.id = this.tableId;

            this.jsOptions = jsOptions;
            this.dtInstance = null;

            this._init();

            SmartDtRegistry[this.tableId] = this;
        }

        _init() {

            const attrConfig = this._readAttributes();
            const columns = this._generateColumnsFromThead();
            const buttons = this._buildButtons();

            const defaultConfig = {

                processing: true,
                autoWidth: false,
                searchDelay: 500,
                order: attrConfig.order,
                pageLength: attrConfig.pageLength,
                searching: attrConfig.searching,

                ajax: {
                    url: attrConfig.url,
                    type: attrConfig.method,
                    contentType: 'application/json',

                    // مهم‌ترین بخش:
                    dataSrc: attrConfig.dataSrc || 'data',     // اگر serverSide باشه undefined میشه → رفتار پیش‌فرض

                    data: (d) => {
                        this._appendExternalFilters(d);
                        if (typeof this.jsOptions.ajaxData === 'function') {
                            this.jsOptions.ajaxData(d);
                        }
                        return JSON.stringify(d);
                    }
                },
                serverSide: attrConfig.serverSide,

                columns: columns,
                buttons: buttons,
                language: { url: '\\common\\lib\\datatable\\fa.json' }
            };

            const finalConfig = $.extend(true, {}, defaultConfig, this.jsOptions);

            delete finalConfig.extraButtons;
            delete finalConfig.ajaxData;

            this.dtInstance = this.$table.DataTable(finalConfig);
            //this._fixSearchUI();
            //this._applyColumnWidths();
            this.searchableColumns = [];
            const self = this;

            this._bindReloadTriggers(attrConfig.reloadOn);
            this._bindAutoSearch();
        }

        _readAttributes() {
            const el = this.$table[0];

            const serverSideAttr = el.dataset.dtServerSide;
            // اگر "true" نوشته شده → serverSide
            // اگر "false" یا اصلاً نوشته نشده → clientSide
            const isServerSide = serverSideAttr === 'true';

            return {
                url: el.dataset.dtUrl || '',
                method: (el.dataset.dtMethod || 'POST').toUpperCase(),

                serverSide: isServerSide,

                // برای حالت client-side، dataSrc رو "data" در نظر می‌گیریم
                dataSrc: isServerSide ? undefined : 'data',

                searchDelay: parseInt(el.dataset.dtSearchDelay) || (isServerSide ? 300 : 200),
                pageLength: parseInt(el.dataset.dtPageLength) || 10,
                searching: el.dataset.dtSearching !== 'false',

                // اگر بعداً بخوای کنترل دقیق‌تری داشته باشی:
                clientSearch: el.dataset.dtClientSearch === 'true' || !isServerSide
            };
        }
        _fixSearchUI() {
            const wrapper = $(this.dtInstance.table().container());

            // 1. گرفتن input سرچ
            const $input = wrapper.find('input[type="search"]');

            // 2. ست کردن placeholder
            $input.attr('placeholder', 'جستجو...');

            // 3. حذف label (بدون خراب کردن input)
            wrapper.find('.dataTables_filter label')
                .contents()
                .filter(function () {
                    return this.nodeType === 3; // text node
                })
                .remove();

            // یا اگر کامل میخوای label حذف شه:
            wrapper.find('.dataTables_filter label').contents().unwrap();
        }
        _applyColumnWidths() {
            if (!this.dtInstance) return;

            const api = this.dtInstance;

            this.$table.find('thead th').each(function (index) {
                const $th = $(this);
                let width = $th.data('dt-width');   // مثلاً "60px" یا "80px"

                if (!width) return;

                // اعمال روی هدر
                const $header = $(api.column(index).header());
                $header.css({
                    'width': width,
                    'min-width': width,
                    'max-width': width,
                    'white-space': 'nowrap',
                    'overflow': 'hidden',
                    'text-overflow': 'ellipsis'
                });

                // اعمال روی تمام سلول‌های بدنه این ستون
                $(api.column(index).nodes()).css({
                    'width': width,
                    'min-width': width,
                    'max-width': width,
                    'white-space': 'nowrap',
                    'overflow': 'hidden',
                    'text-overflow': 'ellipsis',
                    'text-align': 'center'          // برای کد‌ها بهتر است
                });
            });

            // مهم: جدول را به fixed layout تغییر بده
            this.$table.css('table-layout', 'fixed');
        }
        _generateColumnsFromThead() {

            const columns = [];

            this.$table.find('thead th').each(function () {

                const th = $(this);

                const col = {
                    data: th.data('dt-data') || null,
                    title: th.clone().children('template').remove().end().text().trim(),
                    searchable: th.data('dt-searchable') !== false,
                    orderable: th.data('dt-orderable') !== false,
                    className: th.data('dt-class') || '',
                    width: th.data('dt-width') || undefined
                };

                const templateEl = th.find('template')[0];

                if (templateEl) {

                    const rawHtml = templateEl.innerHTML;

                    col.render = function (data, type, row) {

                        return rawHtml.replace(/\{\{([\w.]+)\}\}/g, (match, path) => {

                            const parts = path.split('.');
                            const root = parts[0];

                            if (root === 'data') return data ?? '';
                            if (root === 'row')
                                return parts.slice(1).reduce((o, k) => o?.[k] ?? '', row);

                            return path.split('.').reduce((o, k) => o?.[k] ?? '', row);
                        });
                    };

                } else {

                    const type = th.data('dt-type');
                    if (type) {
                        const renderer = ColumnTypeRenderers.get(type);
                        if (renderer) col.render = data => renderer(data);
                    }
                }

                columns.push(col);
            });

            return columns;
        }

        _buildButtons() {

            const attrKeys = this.$table[0].dataset.dtButtons
                ? this.$table[0].dataset.dtButtons.split(',').map(s => s.trim())
                : [];

            if (!attrKeys.length) return [];

            const innerButtons = [];
            const self = this;

            attrKeys.forEach(key => {

                const factory = DefaultButtons.get(key);
                if (!factory) return;

                const resolved = factory(null);

                innerButtons.push({
                    text: resolved.text,
                    className: 'dropdown-item',
                    action: (e, dt, n, c) =>
                        factory(self.dtInstance).action(e, dt, n, c)
                });

            });

            return [{
                extend: 'collection',
                text: 'خروجی',
                className: 'btn btn-primary btn-sm',
                buttons: innerButtons
            }];
        }

        _appendExternalFilters(d) {
            const tableSelector = '#' + this.tableId;
            const tableClasses = this.$table.attr('class') || '';

           // console.group('🔍 SmartDataTable - Fetching Filters');
           // console.log('Table ID:', this.tableId);
           // console.log('Table Classes:', tableClasses);

            // جمع‌آوری همه selectors ممکن برای target
            const targets = [tableSelector];

            // اضافه کردن کلاس‌های جدول به عنوان target (اگر کلاس خاصی داشته باشد)
            if (tableClasses) {
                tableClasses.split(' ').forEach(cls => {
                    if (cls && cls !== 'table' && cls !== 'min-w-full') {
                        targets.push(`.${cls}`);
                    }
                });
            }

            //console.log('Target selectors:', targets);

            let foundFilters = [];

            // روش اول و سوم: فیلدهای با name یا data-dt-param که target دارند
            targets.forEach(target => {
                // فیلدهای با name
                $(`[data-dt-target="${target}"][name]`).each(function () {
                    const $this = $(this);
                    const name = $this.attr('name');
                    const value = $this.val();
                    const tagName = $this.prop('tagName');
                    const inputType = $this.attr('type') || 'text';

                    d[name] = value;
                    foundFilters.push({
                        method: 'روش اول (name + target)',
                        target: target,
                        element: tagName,
                        type: inputType,
                        name: name,
                        value: value
                    });
                });

                // فیلدهای با data-dt-param
                $(`[data-dt-param][data-dt-target="${target}"]`).each(function () {
                    const $this = $(this);
                    const paramName = $this.data('dt-param');
                    const value = $this.val();
                    const tagName = $this.prop('tagName');
                    const inputType = $this.attr('type') || 'text';

                    if (paramName) {
                        d[paramName] = value;
                        foundFilters.push({
                            method: 'روش سوم (data-dt-param + target)',
                            target: target,
                            element: tagName,
                            type: inputType,
                            paramName: paramName,
                            value: value
                        });
                    }
                });
            });

            // روش دوم: استفاده از data-dt-filters
            const filtersSelector = this.$table.data('dt-filters');
           // console.log('data-dt-filters selector:', filtersSelector);

            if (filtersSelector) {
                $(filtersSelector).each(function () {
                    const $this = $(this);
                    const name = $this.attr('name');
                    const value = $this.val();
                    const tagName = $this.prop('tagName');
                    const inputType = $this.attr('type') || 'text';

                    if (name) {
                        d[name] = value;
                        foundFilters.push({
                            method: 'روش دوم (data-dt-filters)',
                            selector: filtersSelector,
                            element: tagName,
                            type: inputType,
                            name: name,
                            value: value
                        });
                    } else {
                       // console.warn('⚠️ عنصر در data-dt-filters فاقد attribute name است:', $this);
                    }
                });
            }

            //// چاپ نتایج در کنسول
            //if (foundFilters.length > 0) {
            //    console.log('✅ تعداد فیلترهای پیدا شده:', foundFilters.length);
            //    console.table(foundFilters);

            //    // چاپ با جزئیات بیشتر
            //    foundFilters.forEach((filter, index) => {
            //        console.log(`[${index + 1}] ${filter.method}:`, {
            //            'عنصر': filter.element,
            //            'نوع': filter.type,
            //            'پارامتر': filter.name || filter.paramName,
            //            'مقدار': filter.value,
            //            'target': filter.target || filter.selector
            //        });
            //    });
            //} else {
            //    console.warn('❌ هیچ فیلتری پیدا نشد!');
            //    console.log('🔍 نکات بررسی:');
            //    console.log('   - آیا فیلترها دارای attribute "name" یا "data-dt-param" هستند؟');
            //    console.log('   - آیا فیلترها دارای "data-dt-target" صحیح هستند؟');
            //    console.log('   - آیا "data-dt-filters" در جدول تنظیم شده است؟');
            //}

            //console.log('📤 داده‌های نهایی ارسالی به سرور:', d);
            //console.groupEnd();
        }
        _bindReloadTriggers(selector) {

            if (!selector) return;

            const self = this;

            $(document).on('click', selector, function () {
                self.reload();
            });
        }

        _bindAutoSearch() {

            const self = this;
            const tableSelector = '#' + this.tableId;

            let timer;

            $(document).on('input change',
                `[data-dt-auto-search][data-dt-target="${tableSelector}"]`,
                function () {

                    clearTimeout(timer);

                    timer = setTimeout(() => {
                        SmartDtFireEvent(self.tableId, "search", $(this).val());
                    }, 500);

                });
        }

        reload(resetPaging = false) {
            this.dtInstance.ajax.reload(null, resetPaging);
        }

        getApi() {
            return this.dtInstance;
        }

    }

    // ───────────────── Global Event API ─────────────────

    window.SmartDtFireEvent = function (tableId, eventName, payload) {

        const table = SmartDtRegistry[tableId];

        if (!table) {
            console.warn("SmartDT table not found:", tableId);
            return;
        }

        switch (eventName) {

            case "reload":
                table.reload(false);
                break;

            case "reload-reset":
                table.reload(true);
                break;

            case "search":

                const api = table.getApi();

                table._searchValue = (payload || '').toLowerCase();

                api.draw();

                break;
            case "page":
                table.getApi().page(payload).draw('page');
                break;

            default:
                console.warn("Unknown SmartDT event:", eventName);
        }
    };

    // ───────────────── Manual Init ─────────────────

    window.initSmartDataTable = function (selector, jsOptions = {}) {

        const el = document.querySelector(selector);

        if (!el) {
            console.warn("SmartDataTable element not found:", selector);
            return null;
        }

        const instance = new SmartDataTable(el, jsOptions);

        return instance.getApi();
    };

    // ───────────────── Auto Init ─────────────────

    function autoInitSmartTables(targetNode = document) {
        // اسکن تمام جدول‌های جدید در targetNode
        targetNode.querySelectorAll('table[data-smart-dt]').forEach(table => {

            if (!table.id)
                table.id = 'sdt_' + Math.random().toString(36).slice(2);

            if (SmartDtRegistry[table.id]) return;

            new SmartDataTable(table);
        });
    }
    document.addEventListener("DOMContentLoaded", function () {
        autoInitSmartTables();
        setupMutationObserver(); // الان تابع تعریف شده و قابل استفاده است
    });

    // ───────────────── Custom Search (Global) ─────────────────

    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {

        const table = SmartDtRegistry[settings.nTable.id];
        if (!table || !table._searchValue) return true;

        const val = table._searchValue.toLowerCase();

        return table.searchableColumns.some(i => {
            return (data[i] || '').toString().toLowerCase().includes(val);
        });

    });

    function setupMutationObserver() {
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                mutation.addedNodes.forEach((node) => {
                    if (node.nodeType === 1) {
                        if (node.matches && node.matches('table[data-smart-dt]')) {
                            autoInitSmartTables(node);
                        }
                        if (node.querySelectorAll) {
                            autoInitSmartTables(node);
                        }
                    }
                });
            });
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    // ───────────────── Public API ─────────────────

    window.SmartDataTable = {

        registerType(type, fn) {
            ColumnTypeRenderers.register(type, fn);
        },

        registerButton(key, factory) {
            DefaultButtons.register(key, factory);
        }

        // متد refresh دیگه نیازی نیست برای حالت خودکار، ولی اگر خواستی بمونه اشکال نداره
        // refresh(container = document) {
        //     autoInitSmartTables(container);
        // }

    };

})();

