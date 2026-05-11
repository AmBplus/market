# 🚀 AmbMsg Quick Reference for AI Agents
> **Lightweight Modal/Toast Library** | Zero Dependencies | Bootstrap 5.3 Compatible
---
## 📦 Installation
```html
<link rel="stylesheet" href="ambmsg.css">
<script src="ambmsg.js"></script>
```
---
## 🎯 Three Ways to Use

| Method | Best For | Example |
|--------|----------|---------|
| **HTML Attributes** | Static content, server-side rendering | `<button data-amb-toggle="modal" data-amb-target="#modal">Open</button>` |
| **JavaScript API** | Dynamic content, complex logic | `AmbMsg.open('id', {title:'Title', body:'Content'})` |
| **Auto Containers** | Queued messages, validation errors | `<div class="amb-toast-container"><div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Error"></div></div>` |
---
## 📋 Quick Examples
### 1. HTML Attribute (No JS)
```html
<!-- Trigger -->
<button data-amb-toggle="modal" data-amb-target="#myModal">Open</button>
<!-- Modal -->
<div id="myModal" style="display:none;" 
     data-amb-title="Title"
     data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>
    Content here
</div>
```
### 2. JavaScript
```javascript
// Toast
AmbMsg.AmbToast.success('Saved!', 3000);
// Alert
AmbMsg.error('Error', 'Something went wrong');
// Confirm
AmbMsg.confirm({
    title: 'Delete?',
    message: 'Are you sure?',
    onConfirm: () => console.log('Deleted')
});
// Modal
AmbMsg.open('modal', {
    title: 'My Modal',
    body: '<p>Content</p>',
    size: 'md'
});
```
### 3. Auto Container
```html
<!-- Toast queue -->
<div class="amb-toast-container" data-amb-toast-step="300">
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Error 1"></div>
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="Success!"></div>
</div>
<!-- Alert queue -->
<div class="amb-alert-container" data-amb-alert-step="500">
    <div class="amb-alert-item" data-amb-alert-type="error" data-amb-alert-title="Error" data-amb-alert-msg="Failed"></div>
</div>
```
---
## 🔧 Core Options
| Option | Values | Default |
|--------|--------|---------|
| `size` | `sm` `md` `lg` `xl` `xxl` `full` | `md` |
| `position` | `center` `top-left` `top-right` `bottom-left` `bottom-right` etc. | `center` |
| `animOpen` | `fade` `zoom-in` `slide-up` `bounce` `flip-x` `rotate` etc. | `zoom-in` |
| `backdrop` | `true` `false` `static` | `true` |
| `theme` | `''` `dark` | `''` |
| `rtl` | `true` `false` | `false` |
| `draggable` | `true` `false` | `false` |
| `autoClose` | milliseconds | `0` |
---
## 📱 Global Shortcuts (Always Available)
```javascript
// Toasts
ToastSuccess('Message', 3000);
ToastError('Message', 3000);
ToastWarning('Message', 3000);
ToastInfo('Message', 3000);
FireToast('success', 'Message', 3000);
// Alerts
AlertSuccess('Title', 'Message');
AlertError('Title', 'Message');
AlertWarning('Title', 'Message');
AlertInfo('Title', 'Message');
FireAlert('error', 'Title', 'Message', { rtl: true });
```
---
## 🎨 Common Patterns
### Form Validation
```javascript
// Collect errors
const errors = [];
if (!name) errors.push('Name is required');
if (!email) errors.push('Email is required');
// Show as toast queue
errors.forEach(err => ToastError(err, 3000));
```
### Loading State

```javascript
const modal = AmbMsg.open('loading', {
    title: 'Processing',
    body: '<p>Please wait...</p>',
    backdrop: 'static'
});
modal.setLoading(true);
// After async operation
modal.setLoading(false);
modal._body.innerHTML = '<p>✅ Done!</p>';
setTimeout(() => modal.hide(), 2000);
```
### RTL Support (Persian/Arabic)
```javascript
AmbMsg.error('خطا', 'مشکلی پیش آمده', { rtl: true, btnText: 'باشه' });
```
```html
<div data-amb-title="عنوان" data-amb-rtl="true" data-amb-theme="dark">
    محتوای فارسی
</div>
```
---
## 🚦 Toast Types
| Type | Method | Color |
|------|--------|-------|
| Success | `ToastSuccess()` / `AmbMsg.AmbToast.success()` | Green |
| Error | `ToastError()` / `AmbMsg.AmbToast.error()` | Red |
| Warning | `ToastWarning()` / `AmbMsg.AmbToast.warning()` | Orange |
| Info | `ToastInfo()` / `AmbMsg.AmbToast.info()` | Blue |
---
## 📊 Alert Types
| Type | Use Case |
|------|----------|
| `success` | Operation completed, data saved |
| `error` | Validation failed, system error |
| `warning` | Session expiring, caution needed |
| `info` | Information, updates, tips |
---
## 🔄 Complete Flow Example
```javascript
// 1. Confirm action
AmbMsg.confirm({
    title: 'Delete File',
    message: 'Are you sure?',
    onConfirm: async () => {
        // 2. Show loading
        const modal = AmbMsg.open('delete', {
            title: 'Deleting...',
            body: '<p>Please wait</p>',
            backdrop: 'static'
        });
        modal.setLoading(true);
        // 3. Simulate API call
        await new Promise(resolve => setTimeout(resolve, 2000));
        // 4. Show result
        modal.setLoading(false);
        modal._body.innerHTML = '<p>✅ File deleted!</p>';
        setTimeout(() => modal.hide(), 1500);
        // 5. Final confirmation
        setTimeout(() => ToastSuccess('File deleted successfully'), 2000);
    }
});
```
---
## 🎯 Most Used Commands
```javascript
// Show toast
ToastSuccess('Done!');
// Show error alert
AlertError('Error', 'Something failed');
// Show modal
AmbMsg.open('id', { title: 'Title', body: 'Content', size: 'md' });
// Show confirm dialog
AmbMsg.confirm({ title: 'Confirm', message: 'Proceed?', onConfirm: () => {} });
// Close modal
AmbMsg.close('id');
// Set global defaults
AmbMsg.config({ theme: 'dark', size: 'lg', position: 'top-center' });
```
---
## 📁 File Structure
```
project/
├── ambmsg.css          # Styles (required)
├── ambmsg.js           # Core library (required)
├── index.html          # Your page
└── docs/               # Documentation (optional)
    ├── 00-quick-reference.md     # This file
    ├── 01-complete-api-documentation.md
    ├── 02-javascript-usage.md
    ├── 03-html-attribute-usage.md
    └── 04-auto-attribute-documentation.md
```
---
## 💡 Pro Tips
1. **Always close modals** with `data-amb-dismiss="modal"` or `AmbMsg.close()`
2. **Use unique IDs** for each modal
3. **Set `backdrop: 'static'`** for important forms to prevent accidental closing
4. **Use auto containers** for server-side generated messages
5. **RTL + dark theme** works perfectly for Persian applications
## ⚡ Quick Debug
| Problem | Solution |
|---------|----------|
| Modal not showing | Check CSS/JS loaded, ID matches |
| Toast not showing | Use `ToastSuccess()` not `AmbMsg.AmbToast.success()`? both work |
| RTL not working | Add `rtl: true` or `data-amb-rtl="true"` |
| Escape key not closing | Set `keyboard: true` (default) |
---
## 🔗 Full Documentation
- **Complete API**: [01-complete-api-documentation.md](01-complete-api-documentation.md)
- **JavaScript**: [02-javascript-usage.md](02-javascript-usage.md)
- **HTML Attributes**: [03-html-attribute-usage.md](03-html-attribute-usage.md)
- **Auto Containers**: [04-auto-attribute-documentation.md](04-auto-attribute-documentation.md)
---
## ✅ Summary for AI Agents
**Core capabilities:**
- Show modal dialogs (customizable size, position, animation)
- Show toast notifications (auto-dismissing)
- Show alerts (success/error/warning/info)
- Show confirm dialogs (yes/no)
- Work with or without JavaScript
- Support RTL (Persian/Arabic) and dark theme
**Most common tasks:**
1. Show error message → `AlertError('Title', 'Message')`
2. Show success notification → `ToastSuccess('Saved!')`
3. Ask for confirmation → `AmbMsg.confirm({...})`
4. Create custom modal → `AmbMsg.open('id', {title, body})`
5. Display validation errors → Auto container or loop toast errors
**Key files needed:** `ambmsg.css` and `ambmsg.js`
**No dependencies** - works out of the box!