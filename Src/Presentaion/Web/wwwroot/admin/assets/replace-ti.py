import os
import re

# مسیر ریشه‌ای که می‌خوای جستجو از اونجا شروع بشه (می‌تونی تغییرش بدی)
root_directory = "."

# پسوندهای فایل‌هایی که می‌خوای بررسی بشن
file_extensions = (".js", ".cshtml", ".css")

# دیکشنری جایگزینی: تگ‌های Themify به Font Awesome
replacements = {
    r'<i class=" ti ti-lock me-1">': r'<i class=" fas fa-lock me-1">',
    r'<i class="text-primary ti ti-dots-vertical">': r'<i class="text-primary fas fa-ellipsis-vertical">',
    r'<i class="text-primary ti ti-pencil">': r'<i class="text-primary fas fa-pencil">',
    r'<i class="ti menu-toggle-icon d-none d-xl-block ti-sm align-middle">': r'<i class="fas menu-toggle-icon d-none d-xl-block fa-sm align-middle">',
    r'<i class="ti ti-alert-circle ti-xs me-2">': r'<i class="fas fa-circle-exclamation fa-xs me-2">',
    r'<i class="ti ti-arrow-down-circle ti-sm">': r'<i class="fas fa-circle-down fa-sm">',
    r'<i class="ti ti-book ti-sm me-2 text-info">': r'<i class="fas fa-book fa-sm me-2 text-info">',
    r'<i class="ti ti-brand-angular ti-md">': r'<i class="fas fa-brands fa-angular fa-md">',
    r'<i class="ti ti-brand-figma ti-md">': r'<i class="fas fa-brands fa-figma fa-md">',
    r'<i class="ti ti-brand-react-native ti-md">': r'<i class="fas fa-brands fa-react fa-md">',
    r'<i class="ti ti-briefcase ti-xs">': r'<i class="fas fa-briefcase fa-xs">',
    r'<i class="ti ti-chart-pie ti-sm">': r'<i class="fas fa-chart-pie fa-sm">',
    r'<i class="ti ti-chart-pie-2 ti-sm">': r'<i class="fas fa-chart-pie fa-sm">',
    r'<i class="ti ti-chevron-left ti-sm">': r'<i class="fas fa-chevron-left fa-sm">',
    r'<i class="ti ti-chevron-right ti-sm">': r'<i class="fas fa-chevron-right fa-sm">',
    r'<i class="ti ti-circle-check ti-sm">': r'<i class="fas fa-circle-check fa-sm">',
    r'<i class="ti ti-circle-filled fs-tiny me-2">': r'<i class="fas fa-circle fa-tiny me-2">',
    r'<i class="ti ti-circle-half-2 ti-sm">': r'<i class="fas fa-circle-half-stroke fa-sm">',
    r'<i class="ti ti-color-swatch ti-md">': r'<i class="fas fa-swatchbook fa-md">',
    r'<i class="ti ti-copy me-1" >': r'<i class="fas fa-copy me-1" >',
    r'<i class="ti ti-copy me-2" >': r'<i class="fas fa-copy me-2" >',
    r'<i class="ti ti-copy me-2">': r'<i class="fas fa-copy me-2">',
    r'<i class="ti ti-device-floppy ti-sm">': r'<i class="fas fa-floppy-disk fa-sm">',
    r'<i class="ti ti-device-gamepad-2 ti-xs">': r'<i class="fas fa-gamepad fa-xs">',
    r'<i class="ti ti-device-laptop ti-sm">': r'<i class="fas fa-laptop fa-sm">',
    r'<i class="ti ti-device-mobile ti-xs">': r'<i class="fas fa-mobile fa-xs">',
    r'<i class="ti ti-device-watch ti-xs">': r'<i class="fas fa-watch fa-xs">',
    r'<i class="ti ti-diamond ti-md">': r'<i class="fas fa-gem fa-md">',
    r'<i class="ti ti-dots me-1 mt-n1">': r'<i class="fas fa-ellipsis me-1 mt-n1">',
    r'<i class="ti ti-dots-vertical me-2">': r'<i class="fas fa-ellipsis-vertical me-2">',
    r'<i class="ti ti-dots-vertical mx-1 ti-sm">': r'<i class="fas fa-ellipsis-vertical mx-1 fa-sm">',
    r'<i class="ti ti-dots-vertical ti-sm lh-1">': r'<i class="fas fa-ellipsis-vertical fa-sm lh-1">',
    r'<i class="ti ti-dots-vertical ti-sm mx-1">': r'<i class="fas fa-ellipsis-vertical fa-sm mx-1">',
    r'<i class="ti ti-dots-vertical ti-sm">': r'<i class="fas fa-ellipsis-vertical fa-sm">',
    r'<i class="ti ti-dots-vertical">': r'<i class="fas fa-ellipsis-vertical">',
    r'<i class="ti ti-download me-1 ti-xs">': r'<i class="fas fa-download me-1 fa-xs">',
    r'<i class="ti ti-download me-1">': r'<i class="fas fa-download me-1">',
    r'<i class="ti ti-edit ti-sm me-2">': r'<i class="fas fa-pen fa-sm me-2">',
    r'<i class="ti ti-edit ti-sm">': r'<i class="fas fa-pen fa-sm">',
    r'<i class="ti ti-edit">': r'<i class="fas fa-pen">',
    r'<i class="ti ti-eye mx-2 ti-sm">': r'<i class="fas fa-eye mx-2 fa-sm">',
    r'<i class="ti ti-eye">': r'<i class="fas fa-eye">',
    r'<i class="ti ti-file me-2" >': r'<i class="fas fa-file me-2" >',
    r'<i class="ti ti-file me-2">': r'<i class="fas fa-file me-2">',
    r'<i class="ti ti-file-code-2 me-2">': r'<i class="fas fa-file-code me-2">',
    r'<i class="ti ti-file-description me-1">': r'<i class="fas fa-file-lines me-1">',
    r'<i class="ti ti-file-description me-2">': r'<i class="fas fa-file-lines me-2">',
    r'<i class="ti ti-file-export me-2">': r'<i class="fas fa-file-export me-2">',
    r'<i class="ti ti-file-export me-sm-1 m">': r'<i class="fas fa-file-export me-sm-1 m">',
    r'<i class="ti ti-file-export me-sm-1">': r'<i class="fas fa-file-export me-sm-1">',
    r'<i class="ti ti-file-spreadsheet me-1">': r'<i class="fas fa-file-excel me-1">',
    r'<i class="ti ti-file-spreadsheet me-2">': r'<i class="fas fa-file-excel me-2">',
    r'<i class="ti ti-file-text me-1" >': r'<i class="fas fa-file-alt me-1" >',
    r'<i class="ti ti-file-text me-1">': r'<i class="fas fa-file-alt me-1">',
    r'<i class="ti ti-file-text me-2" >': r'<i class="fas fa-file-alt me-2" >',
    r'<i class="ti ti-file-text me-2">': r'<i class="fas fa-file-alt me-2">',
    r'<i class="ti ti-home-2 ti-xs">': r'<i class="fas fa-home fa-xs">',
    r'<i class="ti ti-info-circle ti-sm">': r'<i class="fas fa-circle-info fa-sm">',
    r'<i class="ti ti-logout me-2 ti-sm">': r'<i class="fas fa-sign-out fa-sm me-2">',
    r'<i class="ti ti-mail me-2 ti-sm">': r'<i class="fas fa-envelope fa-sm me-2">',
    r'<i class="ti ti-mail mx-2 ti-sm">': r'<i class="fas fa-envelope mx-2 fa-sm">',
    r'<i class="ti ti-menu-2 ti-sm text-heading">': r'<i class="fas fa-bars fa-sm text-heading">',
    r'<i class="ti ti-plus me-0 me-md-2 ti-xs">': r'<i class="fas fa-plus me-0 me-md-2 fa-xs">',
    r'<i class="ti ti-plus me-0 me-sm-1 mb-1 ti-xs">': r'<i class="fas fa-plus me-0 me-sm-1 mb-1 fa-xs">',
    r'<i class="ti ti-plus me-0 me-sm-1 ti-xs ">': r'<i class="fas fa-plus me-0 me-sm-1 fa-xs ">',
    r'<i class="ti ti-plus me-0 me-sm-1 ti-xs">': r'<i class="fas fa-plus me-0 me-sm-1 fa-xs">',
    r'<i class="ti ti-plus me-md-1">': r'<i class="fas fa-plus me-md-1">',
    r'<i class="ti ti-plus me-md-2">': r'<i class="fas fa-plus me-md-2">',
    r'<i class="ti ti-plus me-sm-1">': r'<i class="fas fa-plus me-sm-1">',
    r'<i class="ti ti-plus ti-xs me-0 me-sm-2">': r'<i class="fas fa-plus fa-xs me-0 me-sm-2">',
    r'<i class="ti ti-printer me-1" >': r'<i class="fas fa-print me-1" >',
    r'<i class="ti ti-printer me-2" >': r'<i class="fas fa-print me-2" >',
    r'<i class="ti ti-printer me-2">': r'<i class="fas fa-print me-2">',
    r'<i class="ti ti-refresh ti-lg">': r'<i class="fas fa-sync fa-lg">',
    r'<i class="ti ti-screen-share me-1 ti-xs">': r'<i class="fas fa-desktop fa-xs me-1">',
    r'<i class="ti ti-screen-share me-2">': r'<i class="fas fa-desktop me-2">',
    r'<i class="ti ti-screen-share ti-xs me-2">': r'<i class="fas fa-desktop fa-xs me-2">',
    r'<i class="ti ti-settings ti-sm">': r'<i class="fas fa-gear fa-sm">',
    r'<i class="ti ti-shoe ti-xs">': r'<i class="fas fa-shoe-prints fa-xs">',
    r'<i class="ti ti-thumb-down">': r'<i class="fas fa-thumbs-down">',
    r'<i class="ti ti-thumb-up">': r'<i class="fas fa-thumbs-up">',
    r'<i class="ti ti-trash ti-sm mx-2">': r'<i class="fas fa-trash fa-sm mx-2">',
    r'<i class="ti ti-trash">': r'<i class="fas fa-trash">',
    r'<i class="ti ti-truck">': r'<i class="fas fa-truck">',
    r'<i class="ti ti-user ti-sm">': r'<i class="fas fa-user fa-sm">',
    r'<i class="ti ti-users ti-sm me-2 text-primary">': r'<i class="fas fa-users fa-sm me-2 text-primary">',
    r'<i class="ti ti-video ti-sm me-2 text-danger" >': r'<i class="fas fa-video fa-sm me-2 text-danger" >',
    r'<i class="ti ti-x d-block d-xl-none ti-sm align-middle">': r'<i class="fas fa-times d-block d-xl-none fa-sm align-middle">',
    r'<i class="ti ti-x ti-lg">': r'<i class="fas fa-times fa-lg">',
    r'<i class="ti-xs ti ti-lock me-1">': r'<i class="fa-xs fas fa-lock me-1">',
    r'<i class="ti-xs ti ti-user me-1">': r'<i class="fa-xs fas fa-user me-1">',
    r'<i class="ti-xs ti ti-users  icon-color me-1">': r'<i class="fa-xs fas fa-users  icon-color me-1">',
    r'<i class="ti-xs ti ti-users me-1">': r'<i class="fa-xs fas fa-users me-1">',
    r'<i class=\'dropdown-toggle ti ti-dots-vertical cursor-pointer\' id=\'board-dropdown\' data-bs-toggle=\'dropdown\' aria-haspopup=\'true\' aria-expanded=\'false\'>': r'<i class=\'dropdown-toggle fas fa-ellipsis-vertical cursor-pointer\' id=\'board-dropdown\' data-bs-toggle=\'dropdown\' aria-haspopup=\'true\' aria-expanded=\'false\'>',
    r'<i class=\'dropdown-toggle ti ti-dots-vertical\' id=\'kanban-tasks-item-dropdown\' data-bs-toggle=\'dropdown\' aria-haspopup=\'true\' aria-expanded=\'false\'>': r'<i class=\'dropdown-toggle fas fa-ellipsis-vertical\' id=\'kanban-tasks-item-dropdown\' data-bs-toggle=\'dropdown\' aria-haspopup=\'true\' aria-expanded=\'false\'>',
    r'<i class=\'ti ti-archive ti-xs\' me-1>': r'<i class=\'fas fa-box-archive fa-xs\' me-1>',
    r'<i class=\'ti ti-edit ti-xs\' me-1>': r'<i class=\'fas fa-pen fa-xs\' me-1>',
    r'<i class=\'ti ti-language rounded-circle ti-md\'>': r'<i class=\'fas fa-language rounded-circle fa-md\'>',
    r'<i class=\'ti ti-md\'>': r'<i class=\'fas fa-md\'>',
    r'<i class=\'ti ti-message-dots ti-xs me-1\'>': r'<i class=\'fas fa-comment-dots fa-xs me-1\'>',
    r'<i class=\'ti ti-paperclip ti-xs me-1\'>': r'<i class=\'fas fa-paperclip fa-xs me-1\'>',
    r'<i class=\'ti ti-plus ti-xs text-heading\'>': r'<i class=\'fas fa-plus fa-xs text-heading\'>',
    r'<i class=\'ti ti-trash ti-xs\' me-1>': r'<i class=\'fas fa-trash fa-xs\' me-1>'
}

# تابع برای جستجو و جایگزینی تگ‌ها
def replace_ti_tags(directory):
    modified_files = 0
    for root, _, files in os.walk(directory):
        for file in files:
            if file.endswith(file_extensions):
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, 'r', encoding='utf-8') as f:
                        content = f.read()

                    # انجام جایگزینی‌ها
                    new_content = content
                    file_modified = False
                    for old_tag, new_tag in replacements.items():
                        if old_tag in new_content:
                            new_content = new_content.replace(old_tag, new_tag)
                            file_modified = True

                    # اگه تغییری ایجاد شده، فایل رو بازنویسی کن
                    if file_modified:
                        with open(file_path, 'w', encoding='utf-8') as f:
                            f.write(new_content)
                        print(f"فایل تغییر کرد: {file_path}")
                        modified_files += 1

                except (UnicodeDecodeError, IOError) as e:
                    print(f"خطا در پردازش فایل {file_path}: {e}")

    print(f"تعداد فایل‌های تغییر یافته: {modified_files}")

# اجرای جایگزینی
print("در حال جستجو و جایگزینی...")
replace_ti_tags(root_directory)
print("جایگزینی کامل شد!")
