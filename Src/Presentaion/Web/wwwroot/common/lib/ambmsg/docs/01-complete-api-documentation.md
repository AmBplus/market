# 📚 AmbMsg Complete API Documentation

> **Version 1.0 | MIT License | Zero Dependencies | Bootstrap 5.3 Compatible**

AmbMsg is a lightweight, attribute-driven modal and toast framework with extensive customization options including 12 animations, 9 positions, 6 sizes, dark/light themes, RTL support, draggable modals, and much more.

---

## 📑 Table of Contents

1. [Quick Start](#quick-start)
2. [Core Features](#core-features)
3. [Modal Component](#modal-component)
   - [Modal Options](#modal-options)
   - [Modal Methods](#modal-methods)
   - [Modal Events](#modal-events)
4. [Toast Component](#toast-component)
   - [Toast Methods](#toast-methods)
   - [Toast Options](#toast-options)
5. [Alert Components](#alert-components)
   - [Alert Types](#alert-types)
   - [Alert with Message List](#alert-with-message-list)
6. [Confirm Dialog](#confirm-dialog)
7. [Animation Gallery](#animation-gallery)
8. [Position Gallery](#position-gallery)
9. [Size Gallery](#size-gallery)
10. [Themes](#themes)
11. [RTL Support](#rtl-support)
12. [Draggable Modals](#draggable-modals)
13. [Loading States](#loading-states)
14. [Callbacks](#callbacks)
15. [Customization](#customization)

---

## Quick Start

```html
<!-- 1. Include CSS -->
<link rel="stylesheet" href="ambmsg.css">

<!-- 2. Include JavaScript -->
<script src="ambmsg.js"></script>

<!-- 3. Use anywhere -->
<button data-amb-toggle="modal" data-amb-target="#myModal">
    Open Modal
</button>
```

---

## Core Features

- ✅ **Zero Dependencies** - Pure vanilla JavaScript
- ✅ **Bootstrap Compatible API** - Same methods as Bootstrap 5.3
- ✅ **Attribute-Driven** - Use HTML attributes or JavaScript
- ✅ **12 Animations** - Fade, zoom, slide, flip, bounce, rotate
- ✅ **9 Positions** - Any corner, edge, or center
- ✅ **6 Sizes** - sm, md, lg, xl, xxl, full
- ✅ **Dark/Light Themes** - Automatic or manual
- ✅ **RTL Support** - Full right-to-left layout
- ✅ **Draggable** - Move modals by dragging header
- ✅ **Toast Notifications** - Auto-dismissing popups
- ✅ **Alert Types** - Success, error, warning, info
- ✅ **Responsive** - Mobile breakpoint support
- ✅ **Keyboard Accessible** - Escape key, tab trapping
- ✅ **Event System** - Complete lifecycle events

---

## Modal Component

### Modal Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `title` | string | `''` | Modal title (HTML allowed) |
| `body` | string | `''` | Main content (HTML allowed) |
| `footer` | string | `''` | Footer content (HTML allowed) |
| `size` | string | `'md'` | `sm`, `md`, `lg`, `xl`, `xxl`, `full`, `custom` |
| `position` | string | `'center'` | 9 positions (see gallery) |
| `animOpen` | string | `'zoom-in'` | Opening animation |
| `animClose` | string | `'zoom-out'` | Closing animation |
| `duration` | number | `300` | Animation duration (ms) |
| `easing` | string | `'ease-in-out'` | CSS easing function |
| `backdrop` | boolean/string | `true` | `true`, `'static'`, or `false` |
| `backdropColor` | string | `''` | Custom backdrop color |
| `keyboard` | boolean | `true` | Close with Escape key |
| `autoFocus` | boolean | `true` | Auto-focus first element |
| `autoClose` | number | `0` | Auto-close after ms (0=off) |
| `draggable` | boolean | `false` | Drag by header |
| `rtl` | boolean | `false` | Right-to-left layout |
| `theme` | string | `''` | `''` (light) or `'dark'` |
| `width` | string | `''` | Custom width (e.g., `'650px'`) |
| `mobileBreakpoint` | number | `576` | Mobile breakpoint (px) |
| `mobileSize` | string | `''` | Size on mobile (e.g., `'full'`) |
| `noDefaultStyle` | boolean | `false` | Remove default styles |
| `alertType` | string | `''` | `success`, `error`, `warning`, `info` |

### Modal Methods

```javascript
// Global methods
AmbMsg.open(id, options);      // Create/show modal
AmbMsg.create(id, options);    // Create only (hidden)
AmbMsg.close(id);              // Close modal
AmbMsg.destroy(id);            // Remove completely
AmbMsg.config(options);        // Update defaults

// Instance methods
const modal = AmbMsg.open('my-modal');
modal.show();                  // Show modal
modal.hide();                  // Hide modal
modal.toggle();                // Toggle visibility
modal.dispose();               // Destroy instance
modal.setLoading(true/false);  // Show/hide loading
modal.config(options);         // Update config

// Properties
modal.isVisible;               // Boolean
modal.element;                 // Wrapper element
modal.id;                      // Modal ID
```

### Modal Events

```javascript
const wrapper = modal.element;

// Lifecycle events
wrapper.addEventListener('show.amb.modal', (e) => {
    console.log('Modal opening...');
    // e.preventDefault() cancels opening
});

wrapper.addEventListener('shown.amb.modal', (e) => {
    console.log('Modal fully opened');
});

wrapper.addEventListener('hide.amb.modal', (e) => {
    console.log('Modal closing...');
    // e.preventDefault() cancels closing
});

wrapper.addEventListener('hidden.amb.modal', (e) => {
    console.log('Modal fully closed');
});

wrapper.addEventListener('backdropclick.amb.modal', (e) => {
    console.log('Backdrop clicked');
});
```

---

## Toast Component

Toast notifications are auto-dismissing popups that appear in the corner of the screen.

### Toast Methods

```javascript
// Basic toast
AmbMsg.AmbToast.show(message, type, duration);

// Type-specific shortcuts
AmbMsg.AmbToast.success(message, duration);
AmbMsg.AmbToast.error(message, duration);
AmbMsg.AmbToast.warning(message, duration);
AmbMsg.AmbToast.info(message, duration);

// Global shortcuts (available globally)
ToastSuccess(message, duration);
ToastError(message, duration);
ToastWarning(message, duration);
ToastInfo(message, duration);
FireToast(type, message, duration);
```

### Toast Options

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `message` | string | Required | Toast message text |
| `type` | string | `'info'` | `success`, `error`, `warning`, `info` |
| `duration` | number | `5000` | Auto-dismiss time (ms), 0 = no auto-dismiss |

### Toast Examples

```javascript
// Simple toast
AmbMsg.AmbToast.success('File saved successfully!', 3000);

// Error toast
AmbMsg.AmbToast.error('Connection failed!', 4000);

// Warning toast
AmbMsg.AmbToast.warning('Session expiring soon!', 2000);

// Info toast
AmbMsg.AmbToast.info('New version available', 5000);

// Custom type
AmbMsg.AmbToast.show('Custom message', 'success', 2500);

// Using global shortcuts
ToastSuccess('Operation completed!');
ToastError('Something went wrong!');
```

---

## Alert Components

Alert dialogs are modal popups with styled headers based on type.

### Alert Types

```javascript
// Basic alert
AmbMsg.alert({
    type: 'success',        // 'success', 'error', 'warning', 'info'
    message: 'Operation successful!',
    title: 'Success',       // Optional, auto-generated if omitted
    btnText: 'OK',          // Button text
    size: 'md',             // Modal size
    animOpen: 'bounce',     // Animation
    animClose: 'zoom-out',  // Closing animation
    theme: '',              // '' or 'dark'
    rtl: false,             // Right-to-left
    autoClose: 0            // Auto-close after ms
});

// Type shortcuts
AmbMsg.success('Success!', 'Operation completed!');
AmbMsg.error('Error!', 'Something went wrong!');
AmbMsg.warning('Warning!', 'Please be careful!');
AmbMsg.info('Info', 'New update available');

// Global shortcuts (available globally)
AlertSuccess('Success!', 'Operation completed!');
AlertError('Error!', 'Something went wrong!');
AlertWarning('Warning!', 'Please be careful!');
AlertInfo('Info', 'New update available');
FireAlert(type, title, messages, options);
```

### Alert with Message List

```javascript
// Single message (string)
AmbMsg.success('Saved!', 'File has been saved successfully');

// Multiple messages (array)
AmbMsg.error(
    'Validation Errors',
    [
        'Username is required',
        'Email must be valid',
        'Password must be at least 8 characters'
    ],
    {
        rtl: true,
        theme: 'dark',
        btnText: 'Got it'
    }
);

// Using global shortcut
AlertError('خطاهای اعتبارسنجی', [
    'نام کاربری الزامی است',
    'ایمیل نامعتبر است'
], { rtl: true, btnText: 'باشه' });
```

### Alert Examples

```javascript
// Success alert
AmbMsg.alert({
    type: 'success',
    message: 'Your profile has been updated!',
    autoClose: 2000
});

// Error alert with custom button
AmbMsg.error('Connection Error', 'Unable to connect to server', {
    btnText: 'Try Again',
    theme: 'dark'
});

// Warning alert with RTL
AlertWarning('هشدار', 'جلسه شما به زودی منقضی می‌شود', {
    rtl: true,
    size: 'sm'
});

// Info alert with custom animation
AmbMsg.info('Information', 'New features available', {
    animOpen: 'flip-x',
    size: 'lg'
});
```

---

## Confirm Dialog

```javascript
AmbMsg.confirm({
    title: 'Delete Item',
    message: 'Are you sure you want to delete this item?',
    confirmText: 'Yes, Delete',   // Default: 'Confirm'
    cancelText: 'Cancel',          // Default: 'Cancel'
    size: 'sm',                    // Modal size
    animOpen: 'zoom-in',           // Animation
    theme: '',                     // '' or 'dark'
    rtl: false,                    // Right-to-left
    onConfirm: () => {
        console.log('Confirmed!');
        // Perform delete action
    },
    onCancel: () => {
        console.log('Cancelled!');
    }
});
```

---

## Animation Gallery

### Available Animations

| Animation Name | Description | CSS Class |
|----------------|-------------|-----------|
| `fade` | Simple opacity fade | `amb-anim-fade` |
| `zoom-in` | Scale from 0.7 | `amb-anim-zoom-in` |
| `zoom-out` | Scale from 1.3 | `amb-anim-zoom-out` |
| `slide-up` | From bottom | `amb-anim-slide-up` |
| `slide-down` | From top | `amb-anim-slide-down` |
| `slide-left` | From right | `amb-anim-slide-left` |
| `slide-right` | From left | `amb-anim-slide-right` |
| `flip-x` | 3D X-axis rotation | `amb-anim-flip-x` |
| `flip-y` | 3D Y-axis rotation | `amb-anim-flip-y` |
| `bounce` | Elastic bounce | `amb-anim-bounce` |
| `rotate` | Rotation + scale | `amb-anim-rotate` |
| `none` | No animation | - |

### Animation Examples

```javascript
// Recommended combinations
const animations = {
    // Entrance from bottom, exit to bottom
    slide: { animOpen: 'slide-up', animClose: 'slide-down' },
    
    // Entrance from right, exit to right
    slideHorizontal: { animOpen: 'slide-left', animClose: 'slide-right' },
    
    // Zoom in, fade out
    zoom: { animOpen: 'zoom-in', animClose: 'fade' },
    
    // 3D flip entrance, 3D flip exit
    flip: { animOpen: 'flip-x', animClose: 'flip-y' },
    
    // Bounce entrance, fade exit
    bounce: { animOpen: 'bounce', animClose: 'fade' }
};

// Usage
AmbMsg.open('modal-id', {
    ...animations.slide,
    title: 'Animated Modal'
});
```

---

## Position Gallery

### 9 Available Positions

```
┌─────────────┬─────────────┬─────────────┐
│  top-left   │ top-center  │  top-right  │
├─────────────┼─────────────┼─────────────┤
│ center-left │   center    │ center-right│
├─────────────┼─────────────┼─────────────┤
│bottom-left  │bottom-center│ bottom-right│
└─────────────┴─────────────┴─────────────┘
```

### Position Examples

```javascript
// Center (default)
{ position: 'center' }

// Top right (good for notifications)
{ position: 'top-right' }

// Bottom center (good for mobile)
{ position: 'bottom-center' }

// Custom positions
{ position: 'top-left' }
{ position: 'center-left' }
{ position: 'bottom-right' }
```

---

## Size Gallery

| Size | Max Width | Use Case |
|------|-----------|----------|
| `sm` | 300px | Confirmations, alerts |
| `md` | 500px | Default, most cases |
| `lg` | 800px | Large forms |
| `xl` | 1140px | Heavy content |
| `xxl` | 1400px | Large tables |
| `full` | 100% | Fullscreen |
| `custom` | Custom | Use with `width` option |

### Size Examples

```javascript
// Small modal
{ size: 'sm' }

// Large modal
{ size: 'lg' }

// Fullscreen
{ size: 'full' }

// Custom width
{ size: 'custom', width: '650px' }

// Responsive (full on mobile, lg on desktop)
{
    size: 'lg',
    mobileSize: 'full',
    mobileBreakpoint: 768
}
```

---

## Themes

### Light Theme (Default)
```javascript
{ theme: '' }  // or omit
```

### Dark Theme
```javascript
{ theme: 'dark' }
```

### Theme Examples

```javascript
// Dark modal
AmbMsg.open('dark-modal', {
    title: 'Dark Theme',
    body: '<p>This modal uses dark theme</p>',
    theme: 'dark'
});

// Dark alert
AmbMsg.error('Error', 'System failure', {
    theme: 'dark',
    rtl: true
});

// Toggle theme via attribute
<div data-amb-theme="dark">...</div>
```

### CSS Custom Variables

```css
:root {
    /* Light theme */
    --amb-bg: #fff;
    --amb-color: #212529;
    --amb-border: #dee2e6;
    --amb-header-bg: #f8f9fa;
    --amb-footer-bg: #f8f9fa;
    
    /* Dark theme */
    /* .amb-dark overrides these */
}
```

---

## RTL Support

```javascript
// Enable RTL
{ rtl: true }

// Example with RTL
AmbMsg.open('rtl-modal', {
    title: 'عنوان فارسی',
    body: '<p>این متن به صورت راست‌چین نمایش داده می‌شود</p>',
    rtl: true,
    theme: 'dark'
});

// Alert with RTL
AlertError('خطا', 'مشکلی پیش آمده است', {
    rtl: true,
    btnText: 'باشه'
});
```

---

## Draggable Modals

```javascript
// Enable dragging
{ draggable: true }

// Example
AmbMsg.open('draggable-modal', {
    title: 'Drag Me!',
    body: '<p>Drag this modal by its header</p>',
    draggable: true,
    size: 'md'
});

// Via attribute
<div id="drag-modal" data-amb-draggable="true">
    <p>Content</p>
</div>
```

---

## Loading States

```javascript
const modal = AmbMsg.open('loading-demo', {
    title: 'Loading Data',
    body: '<p>Please wait...</p>',
    backdrop: 'static',
    keyboard: false
});

// Show loading spinner
modal.setLoading(true);

// Simulate async operation
setTimeout(() => {
    modal.setLoading(false);
    modal._body.innerHTML = '<p>✅ Data loaded successfully!</p>';
    // Add footer button if needed
    const footer = document.createElement('div');
    footer.className = 'amb-footer';
    footer.innerHTML = '<button class="amb-btn amb-btn-primary" data-amb-dismiss="modal">Close</button>';
    modal._dialog.appendChild(footer);
}, 3000);
```

---

## Callbacks

```javascript
AmbMsg.open('callback-modal', {
    title: 'Modal with Callbacks',
    body: '<p>This modal has lifecycle callbacks</p>',
    
    // Before show
    onload: (modal) => {
        console.log('Starting to open...');
    },
    
    // After animation complete
    afterload: (modal) => {
        console.log('Fully opened!');
    },
    
    // Before hide (return false to cancel)
    onclose: (modal) => {
        const shouldClose = confirm('Close this modal?');
        return shouldClose;
    },
    
    // After hide animation complete
    afterclose: (modal) => {
        console.log('Modal closed');
    },
    
    // Backdrop click
    onbackdropclick: (modal) => {
        console.log('Backdrop clicked');
    }
});
```

---

## Customization

### CSS Variable Overrides

```css
/* Override globally */
:root {
    --amb-radius: 1rem;           /* Rounder corners */
    --amb-shadow: 0 20px 60px rgba(0,0,0,.3);  /* Stronger shadow */
    --amb-duration: 500ms;        /* Slower animations */
}

/* Override for specific modal */
.custom-modal .amb-dialog {
    --amb-radius: 0;
    --amb-shadow: none;
}
```

### Style Classes

```css
.amb-backdrop      /* Backdrop overlay */
.amb-wrapper       /* Main wrapper */
.amb-dialog        /* Modal dialog */
.amb-header        /* Header section */
.amb-body          /* Body content */
.amb-footer        /* Footer section */
.amb-title         /* Title text */
.amb-close         /* Close button */
.amb-draggable     /* Draggable modal */
.amb-loading       /* Loading overlay */
.amb-spinner       /* Spinner animation */
```

### No Default Style

```javascript
// Remove all default styles (you provide your own)
{ noDefaultStyle: true }

// Example
AmbMsg.open('custom-styled', {
    title: 'Custom Styles',
    body: '<p>You must provide your own CSS</p>',
    noDefaultStyle: true
});
```

---

## Bootstrap Compatibility

AmbMsg shares the same API as Bootstrap 5.3 Modal:

```javascript
// Bootstrap equivalent
const modal = bootstrap.Modal.getInstance(element);
modal.show();

// AmbMsg equivalent
const modal = AmbMsg.Modal.getInstance(element);
modal.show();

// Get or create instance
const modal = AmbMsg.Modal.getOrCreateInstance(element, options);

// All methods match Bootstrap
modal.show();
modal.hide();
modal.toggle();
modal.dispose();
```

---

## Browser Support

- ✅ Chrome 90+
- ✅ Firefox 88+
- ✅ Safari 14+
- ✅ Edge 90+
- ✅ Opera 76+

---

## License

MIT License - Free for personal and commercial use