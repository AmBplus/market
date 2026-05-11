# 🟩 HTML Attribute Usage Documentation

> Complete guide for using AmbMsg with HTML `data-amb-*` attributes (no JavaScript required)

---

## 📑 Table of Contents

1. [Quick Start](#quick-start)
2. [Modal Attributes](#modal-attributes)
   - [Creating Modals](#creating-modals)
   - [Trigger Buttons](#trigger-buttons)
   - [Dismiss Buttons](#dismiss-buttons)
3. [All Data Attributes](#all-data-attributes)
4. [Complete Examples](#complete-examples)
   - [Basic Modal](#basic-modal)
   - [Modal with All Options](#modal-with-all-options)
   - [Dark Theme + RTL](#dark-theme--rtl)
   - [Draggable Modal](#draggable-modal)
   - [Static Backdrop Form](#static-backdrop-form)
5. [Toast with Attributes](#toast-with-attributes)
6. [Alert with Attributes](#alert-with-attributes)
7. [Best Practices](#best-practices)

---

## Quick Start

With AmbMsg, you can create fully functional modals using only HTML attributes - no JavaScript needed!

```html
<!-- 1. Include CSS and JS -->
<link rel="stylesheet" href="ambmsg.css">
<script src="ambmsg.js"></script>

<!-- 2. Add trigger button -->
<button data-amb-toggle="modal" data-amb-target="#myModal">
    Open Modal
</button>

<!-- 3. Define modal content -->
<div id="myModal" style="display:none;"
     data-amb-title="My Modal Title"
     data-amb-footer="<button data-amb-dismiss='modal'>Close</button>">
    <p>This modal works without writing any JavaScript!</p>
</div>
```

---

## Modal Attributes

### Creating Modals

Any `<div>` with an `id` and `data-amb-*` attributes becomes a modal:

```html
<div id="modal-id" style="display:none;"
     data-amb-title="Modal Title"
     data-amb-size="md"
     data-amb-position="center">
    <!-- Modal content here -->
</div>
```

### Trigger Buttons

```html
<!-- Primary method -->
<button data-amb-toggle="modal" data-amb-target="#modalId">
    Open Modal
</button>

<!-- Alternative using data-amb-id -->
<button data-amb-toggle="modal" data-amb-id="modalId">
    Open Modal
</button>

<!-- Any element can be a trigger -->
<a href="#" data-amb-toggle="modal" data-amb-target="#modalId">
    Open Modal
</a>
```

### Dismiss Buttons

```html
<!-- Inside modal footer or body -->
<button data-amb-dismiss="modal">
    Close
</button>

<!-- With custom styling -->
<button class="amb-btn amb-btn-primary" data-amb-dismiss="modal">
    Save & Close
</button>
```

---

## All Data Attributes

| Attribute | Values | Default | Description |
|-----------|--------|---------|-------------|
| `data-amb-title` | string | `''` | Modal title (HTML allowed) |
| `data-amb-size` | `sm`, `md`, `lg`, `xl`, `xxl`, `full`, `custom` | `md` | Modal size |
| `data-amb-position` | `center`, `top-left`, `top-center`, `top-right`, `center-left`, `center-right`, `bottom-left`, `bottom-center`, `bottom-right` | `center` | Modal position |
| `data-amb-anim-open` | animation name | `zoom-in` | Opening animation |
| `data-amb-anim-close` | animation name | `zoom-out` | Closing animation |
| `data-amb-duration` | number (ms) | `300` | Animation duration |
| `data-amb-easing` | CSS easing | `ease-in-out` | Animation easing |
| `data-amb-backdrop` | `true`, `false`, `static` | `true` | Backdrop behavior |
| `data-amb-backdrop-color` | CSS color | `''` | Custom backdrop color |
| `data-amb-keyboard` | `true`, `false` | `true` | Escape key behavior |
| `data-amb-auto-focus` | `true`, `false` | `true` | Auto-focus first element |
| `data-amb-auto-close` | number (ms) | `0` | Auto-close delay |
| `data-amb-draggable` | `true`, `false` | `false` | Draggable modal |
| `data-amb-rtl` | `true`, `false` | `false` | Right-to-left layout |
| `data-amb-theme` | `dark`, `''` | `''` | Color theme |
| `data-amb-width` | CSS width | `''` | Custom width |
| `data-amb-mobile-breakpoint` | number (px) | `576` | Mobile breakpoint |
| `data-amb-mobile-size` | size name | `''` | Size on mobile |
| `data-amb-no-default-style` | (present) | `false` | Remove default styles |
| `data-amb-footer` | HTML string | `''` | Footer content |

---

## Complete Examples

### Basic Modal

```html
<!-- Trigger -->
<button data-amb-toggle="modal" data-amb-target="#basicModal">
    Open Basic Modal
</button>

<!-- Modal -->
<div id="basicModal" style="display:none;"
     data-amb-title="Basic Modal"
     data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>
    <p>This is a simple modal with default settings.</p>
</div>
```

### Modal with All Options

```html
<button data-amb-toggle="modal" data-amb-target="#advancedModal">
    Open Advanced Modal
</button>

<div id="advancedModal" style="display:none;"
     data-amb-title="<span style='color:#6366f1;'>✨ Advanced Modal</span>"
     data-amb-size="lg"
     data-amb-position="top-center"
     data-amb-anim-open="bounce"
     data-amb-anim-close="zoom-out"
     data-amb-duration="400"
     data-amb-easing="ease-out"
     data-amb-backdrop="static"
     data-amb-keyboard="false"
     data-amb-auto-focus="true"
     data-amb-draggable="true"
     data-amb-width="700px"
     data-amb-footer='
        <button class="amb-btn amb-btn-outline" data-amb-dismiss="modal">Cancel</button>
        <button class="amb-btn amb-btn-primary" data-amb-dismiss="modal">Confirm</button>
     '>
    
    <h3>Advanced Configuration</h3>
    <p>This modal demonstrates all available options using HTML attributes only.</p>
    <form>
        <label>Name: <input type="text" placeholder="Enter your name"></label>
        <br><br>
        <label>Email: <input type="email" placeholder="Enter your email"></label>
    </form>
</div>
```

### Dark Theme + RTL

```html
<button data-amb-toggle="modal" data-amb-target="#darkRtlModal">
    Open Dark RTL Modal
</button>

<div id="darkRtlModal" style="display:none;"
     data-amb-title="مودال تاریک"
     data-amb-theme="dark"
     data-amb-rtl="true"
     data-amb-size="md"
     data-amb-position="center"
     data-amb-footer='<button class="amb-btn amb-btn-primary" data-amb-dismiss="modal">باشه</button>'>
    
    <p>این مودال با <strong>تم تاریک</strong> و پشتیبانی از زبان فارسی نمایش داده می‌شود.</p>
    <p>✅ تمام متن‌ها به صورت راست‌چین هستند.</p>
</div>
```

### Draggable Modal

```html
<button data-amb-toggle="modal" data-amb-target="#draggableModal">
    Open Draggable Modal
</button>

<div id="draggableModal" style="display:none;"
     data-amb-title="↕️ Drag Me!"
     data-amb-draggable="true"
     data-amb-size="md"
     data-amb-position="center"
     data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>
    
    <p>Click and drag the header to move this modal anywhere on the screen.</p>
    <p>Works with both mouse and touch devices!</p>
</div>
```

### Static Backdrop Form

```html
<button data-amb-toggle="modal" data-amb-target="#formModal">
    Open Form Modal
</button>

<div id="formModal" style="display:none;"
     data-amb-title="Contact Form"
     data-amb-backdrop="static"
     data-amb-keyboard="false"
     data-amb-size="md"
     data-amb-footer='
        <button class="amb-btn amb-btn-outline" data-amb-dismiss="modal">Cancel</button>
        <button class="amb-btn amb-btn-primary" id="submitFormBtn">Submit</button>
     '>
    
    <form id="contactForm">
        <label>Name:</label>
        <input type="text" name="name" required style="width:100%; padding:8px; margin:8px 0;">
        
        <label>Email:</label>
        <input type="email" name="email" required style="width:100%; padding:8px; margin:8px 0;">
        
        <label>Message:</label>
        <textarea name="message" rows="3" style="width:100%; padding:8px; margin:8px 0;"></textarea>
    </form>
    
    <script>
        // You can still add JavaScript for form handling
        document.addEventListener('DOMContentLoaded', () => {
            const submitBtn = document.getElementById('submitFormBtn');
            if (submitBtn) {
                submitBtn.onclick = () => {
                    const name = document.querySelector('#contactForm input[name="name"]').value;
                    if (name) {
                        alert(`Thank you ${name}!`);
                        // Close modal programmatically if needed
                        const modal = AmbMsg.Modal.getInstance('formModal');
                        if (modal) modal.hide();
                    }
                };
            }
        });
    </script>
</div>
```

### Auto-Close Modal

```html
<button data-amb-toggle="modal" data-amb-target="#autoCloseModal">
    Show Notification (Auto-closes)
</button>

<div id="autoCloseModal" style="display:none;"
     data-amb-title="Notification"
     data-amb-auto-close="3000"
     data-amb-size="sm"
     data-amb-position="top-center"
     data-amb-anim-open="slide-down"
     data-amb-anim-close="slide-up">
    
    <p>✅ This modal will automatically close in 3 seconds!</p>
</div>
```

### Responsive Modal

```html
<button data-amb-toggle="modal" data-amb-target="#responsiveModal">
    Open Responsive Modal
</button>

<div id="responsiveModal" style="display:none;"
     data-amb-title="Responsive Modal"
     data-amb-size="lg"
     data-amb-mobile-size="full"
     data-amb-mobile-breakpoint="768"
     data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>
    
    <p>On desktop: Large modal (800px)</p>
    <p>On mobile (≤768px): Fullscreen modal</p>
</div>
```

---

## Toast with Attributes

You can also create toast notifications using HTML attributes:

```html
<!-- Toast container with step delay -->
<div class="amb-toast-container" data-amb-toast-step="300">
    
    <!-- Individual toast items -->
    <div class="amb-toast-item" 
         data-amb-toast-type="success" 
         data-amb-toast-msg="✅ Operation completed successfully!">
    </div>
    
    <div class="amb-toast-item" 
         data-amb-toast-type="error" 
         data-amb-toast-msg="❌ Connection failed!">
    </div>
    
    <div class="amb-toast-item" 
         data-amb-toast-type="warning" 
         data-amb-toast-msg="⚠️ Session expiring soon!">
    </div>
    
    <div class="amb-toast-item" 
         data-amb-toast-type="info" 
         data-amb-toast-msg="ℹ️ New version available">
    </div>
</div>
```

The toasts will appear automatically in sequence with the specified delay between each.

---

## Alert with Attributes

Create alert modals using HTML attributes:

```html
<!-- Alert container with step delay -->
<div class="amb-alert-container" data-amb-alert-step="500">
    
    <!-- Single alert item -->
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="System Error"
         data-amb-alert-msg="Database connection failed"
         data-amb-alert-rtl="false"
         data-amb-alert-theme="dark"
         data-amb-alert-btn="Try Again">
    </div>
    
    <!-- Another alert -->
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="Success"
         data-amb-alert-msg="Data saved successfully!"
         data-amb-alert-auto-close="2000">
    </div>
</div>
```

### Alert Item Attributes

| Attribute | Values | Description |
|-----------|--------|-------------|
| `data-amb-alert-type` | `success`, `error`, `warning`, `info` | Alert type |
| `data-amb-alert-title` | string | Alert title |
| `data-amb-alert-msg` | string | Alert message |
| `data-amb-alert-rtl` | `true`, `false` | RTL layout |
| `data-amb-alert-theme` | `dark`, `''` | Color theme |
| `data-amb-alert-btn` | string | Button text |
| `data-amb-alert-auto-close` | number (ms) | Auto-close delay |

---

## Best Practices

### 1. Always Set Display: None
```html
<div id="myModal" style="display:none;">
    <!-- Content -->
</div>
```

### 2. Use Unique IDs
```html
<!-- Good -->
<div id="modal-contact-form">

<!-- Bad -->
<div id="modal">
```

### 3. HTML in Attributes
Use single quotes for attribute values containing double quotes:

```html
<!-- Correct -->
<div data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>

<!-- Incorrect -->
<div data-amb-footer="<button data-amb-dismiss="modal">Close</button>">
```

### 4. Combine with CSS Classes
```html
<button class="btn btn-primary" data-amb-toggle="modal" data-amb-target="#myModal">
    Open
</button>
```

### 5. Static Backdrop for Important Forms
```html
<div data-amb-backdrop="static" data-amb-keyboard="false">
    <!-- User must explicitly close -->
</div>
```

### 6. Provide Footer Buttons
Always include a way to close the modal:

```html
<div data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>
```

---

## Complete Working Example

```html
<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" href="ambmsg.css">
</head>
<body>

<!-- Triggers -->
<button data-amb-toggle="modal" data-amb-target="#exampleModal">
    Open Example Modal
</button>

<!-- Modal -->
<div id="exampleModal" style="display:none;"
     data-amb-title="Complete Example"
     data-amb-size="lg"
     data-amb-position="center"
     data-amb-anim-open="slide-up"
     data-amb-anim-close="slide-down"
     data-amb-backdrop="static"
     data-amb-draggable="true"
     data-amb-theme="dark"
     data-amb-footer='
        <button class="amb-btn amb-btn-outline" data-amb-dismiss="modal">Cancel</button>
        <button class="amb-btn amb-btn-primary" data-amb-dismiss="modal">OK</button>
     '>
    
    <h3>Welcome to AmbMsg!</h3>
    <p>This modal was created using only HTML attributes.</p>
    <ul>
        <li>✅ No JavaScript required</li>
        <li>✅ Fully customizable</li>
        <li>✅ Responsive design</li>
        <li>✅ Smooth animations</li>
    </ul>
</div>

<script src="ambmsg.js"></script>
</body>
</html>
```

---

## Quick Reference Card

```html
<!-- Modal Structure -->
<div id="unique-id" style="display:none;"
     data-amb-title="Title"
     data-amb-size="md|lg|sm|xl|xxl|full"
     data-amb-position="center|top-left|top-right|bottom-left|bottom-right|..."
     data-amb-anim-open="zoom-in|slide-up|bounce|flip-x|..."
     data-amb-backdrop="true|false|static"
     data-amb-keyboard="true|false"
     data-amb-auto-close="3000"
     data-amb-draggable="true"
     data-amb-theme="dark"
     data-amb-rtl="true"
     data-amb-footer='<button data-amb-dismiss="modal">Close</button>'>
    
    <!-- Modal content -->
    
</div>

<!-- Trigger -->
<button data-amb-toggle="modal" data-amb-target="#unique-id">
    Open Modal
</button>

<!-- Dismiss -->
<button data-amb-dismiss="modal">Close</button>
```

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Modal doesn't appear | Ensure CSS and JS are loaded, check ID match |
| Attributes not working | Verify attribute names: `data-amb-*` exactly |
| Footer buttons not closing | Add `data-amb-dismiss="modal"` to buttons |
| Modal shows on page load | Remove `data-amb-toggle` from modal element |
| Animation not playing | Check animation name spelling |
| Dark theme not applying | Use `data-amb-theme="dark"` |
| RTL not working | Use `data-amb-rtl="true"` |

---

## Next Steps

- Try the [JavaScript API](02-javascript-usage.md) for dynamic control
- See [Auto Attribute Documentation](04-auto-attribute-documentation.md) for automated message queues
- Explore the [Complete API Reference](01-complete-api-documentation.md) for all options