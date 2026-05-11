(function () {
    'use strict';

    const LANG = {
        fa: {
            loading: 'در حال پردازش...',
            confirmTitle: 'تأیید',
            confirmText: 'آیا از انجام این عملیات مطمئن هستید؟',
            confirmOk: 'بله',
            cancel: 'انصراف',
            successTitle: 'موفق',
            successText: 'عملیات با موفقیت انجام شد.',
            errorTitle: 'خطا',
            errorOk: 'باشه'
        },
        en: {
            loading: 'Processing...',
            confirmTitle: 'Confirm',
            confirmText: 'Are you sure you want to perform this action?',
            confirmOk: 'Yes',
            cancel: 'Cancel',
            successTitle: 'Success',
            successText: 'Operation completed successfully.',
            errorTitle: 'Error',
            errorOk: 'OK'
        }
    };

    const D = Object.assign({
        method: 'POST',
        closeAfter: 2500,
        lang: 'fa'
    }, window.AutoDeleteDefaults || {});

    const T = LANG[D.lang] || LANG.fa;

    function reloadTable(tableId) {
        const btn = document.getElementById('sdt-reload-' + tableId);
        if (btn) btn.click();
    }

    function collectParams($btn) {
        const payload = {};

        $.each($btn.data(), function (key, value) {
            if (key.startsWith('adParam')) {
                const param = key.replace('adParam', '');
                const name = param.charAt(0).toLowerCase() + param.slice(1);
                payload[name] = value;
            }
        });

        return payload;
    }

    function fireAjax($btn, payload) {

        const url = $btn.data('ad-url');
        const method = ($btn.data('ad-method') || D.method).toUpperCase();
        const tableId = $btn.data('ad-table');
        const closeAfter = parseInt($btn.data('ad-close-after')) || D.closeAfter;

        Swal.fire({
            title: T.loading,
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false,
            didOpen: () => Swal.showLoading()
        });

        $.ajax({
            url,
            type: method,
            contentType: 'application/json',
            data: JSON.stringify(payload),
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .done(function (res) {

                Swal.fire({
                    title: T.successTitle,
                    text: res?.message || T.successText,
                    icon: 'success',
                    timer: closeAfter,
                    timerProgressBar: true,
                    showConfirmButton: false
                });

                if (tableId) reloadTable(tableId);
            })
            .fail(function (xhr) {

                let msg = T.errorTitle;

                try {
                    msg = JSON.parse(xhr.responseText)?.message || msg;
                } catch { }

                Swal.fire({
                    title: T.errorTitle,
                    html: `<p>${msg}</p><small class="text-muted">Status: ${xhr.status}</small>`,
                    icon: 'error',
                    confirmButtonText: T.errorOk
                });
            });
    }

    $(document).on('click', '[data-ad-url]', function (e) {

        e.preventDefault();

        const $btn = $(this);
        const url = $btn.data('ad-url');

        if (!url) {
            console.warn('[auto-delete] data-ad-url is required.');
            return;
        }

        let payload = {};

        if ($btn.data('ad-payload')) {
            try {
                payload = JSON.parse($btn.data('ad-payload'));
            } catch {
                console.warn('[auto-delete] Invalid JSON payload');
            }
        } else {
            payload = collectParams($btn);
        }

        Swal.fire({
            title: T.confirmTitle,
            text: T.confirmText,
            icon: 'warning',
            showCancelButton: true,
            reverseButtons: true,
            confirmButtonText: T.confirmOk,
            cancelButtonText: T.cancel
        }).then(r => {
            if (r.isConfirmed) fireAjax($btn, payload);
        });

    });

})();
