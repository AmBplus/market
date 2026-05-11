# 🤖 Auto Attribute Documentation

> Automatic modal and toast message queues using HTML containers - perfect for form validation, notifications, and message sequences

---

## 📑 Table of Contents

1. [Overview](#overview)
2. [Toast Auto Container](#toast-auto-container)
3. [Alert Auto Container](#alert-auto-container)
4. [Use Cases](#use-cases)
5. [Server-Side Integration](#server-side-integration)
6. [Best Practices](#best-practices)
7. [Complete Examples](#complete-examples)

---

## Overview

AmbMsg automatically scans the DOM for special containers and converts them into toast notifications or modal alerts. This feature is perfect for:

- **Server-side generated messages** (PHP, ASP.NET, etc.)
- **Form validation errors** displayed one after another
- **Sequential notifications** with timing control
- **Bulk messages** from backend systems
- **Queue-based messaging** without JavaScript

The system automatically:
1. Scans for `.amb-toast-container` and `.amb-alert-container` elements
2. Processes each child item with a configurable delay
3. Displays messages as toasts or modals
4. Removes the containers after processing

---

## Toast Auto Container

Toast containers generate non-modal toast notifications that auto-dismiss.

### Toast Container Structure

```html
<div class="amb-toast-container" data-amb-toast-step="delayInMilliseconds">
    <div class="amb-toast-item" data-amb-toast-type="type" data-amb-toast-msg="message"></div>
    <div class="amb-toast-item" ...></div>
    <!-- More toast items -->
</div>
```

### Toast Item Attributes

| Attribute | Values | Required | Description |
|-----------|--------|----------|-------------|
| `data-amb-toast-type` | `success`, `error`, `warning`, `info` | ✅ Yes | Toast style |
| `data-amb-toast-msg` | string | ✅ Yes | Message text |
| `data-amb-toast-duration` | number (ms) | ❌ No | Custom duration (default: 5000) |

### Container Attributes

| Attribute | Description | Default |
|-----------|-------------|---------|
| `data-amb-toast-step` | Delay between consecutive toasts (ms) | `200` |

### Toast Examples

#### Basic Toast Queue

```html
<!-- Success toasts with 500ms delay -->
<div class="amb-toast-container" data-amb-toast-step="500">
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="✅ File uploaded successfully"></div>
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="✅ Email notification sent"></div>
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="✅ Database updated"></div>
</div>
```

#### Mixed Toast Types (RTL)

```html
<div class="amb-toast-container" data-amb-toast-step="400">
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="ذخیره شد"></div>
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="خطا در اتصال"></div>
    <div class="amb-toast-item" data-amb-toast-type="warning" data-amb-toast-msg="هشدار: زمان کم است"></div>
    <div class="amb-toast-item" data-amb-toast-type="info" data-amb-toast-msg="اطلاعیه جدید"></div>
</div>
```

#### Form Validation Errors

```html
<div class="amb-toast-container" data-amb-toast-step="300">
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Username is required"></div>
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Email must be valid"></div>
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Password must be at least 8 characters"></div>
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Passwords do not match"></div>
</div>
```

#### Custom Duration Per Toast

```html
<div class="amb-toast-container" data-amb-toast-step="200">
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="Quick message" data-amb-toast-duration="2000"></div>
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="Important error - stays longer" data-amb-toast-duration="8000"></div>
    <div class="amb-toast-item" data-amb-toast-type="info" data-amb-toast-msg="Normal duration (default 5000ms)"></div>
</div>
```

---

## Alert Auto Container

Alert containers generate modal dialogs that require user interaction.

### Alert Container Structure

```html
<div class="amb-alert-container" data-amb-alert-step="delayInMilliseconds">
    <div class="amb-alert-item" 
         data-amb-alert-type="type"
         data-amb-alert-title="Title"
         data-amb-alert-msg="Message"
         data-amb-alert-btn="Button Text">
    </div>
    <!-- More alert items -->
</div>
```

### Alert Item Attributes

| Attribute | Values | Required | Description |
|-----------|--------|----------|-------------|
| `data-amb-alert-type` | `success`, `error`, `warning`, `info` | ✅ Yes | Alert style |
| `data-amb-alert-title` | string | ❌ No | Alert title (auto-generated if omitted) |
| `data-amb-alert-msg` | string | ✅ Yes | Alert message |
| `data-amb-alert-btn` | string | ❌ No | Button text (default: 'OK' or 'باشه' for RTL) |
| `data-amb-alert-rtl` | `true`, `false` | ❌ No | RTL layout |
| `data-amb-alert-theme` | `dark`, `''` | ❌ No | Color theme |
| `data-amb-alert-size` | `sm`, `md`, `lg`, `xl`, `xxl`, `full` | ❌ No | Modal size |
| `data-amb-alert-anim` | animation name | ❌ No | Opening animation |
| `data-amb-alert-auto-close` | number (ms) | ❌ No | Auto-close delay |

### Container Attributes

| Attribute | Description | Default |
|-----------|-------------|---------|
| `data-amb-alert-step` | Delay between consecutive alerts (ms) | `200` |

### Alert Examples

#### Single Alert Queue

```html
<div class="amb-alert-container" data-amb-alert-step="300">
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="System Error"
         data-amb-alert-msg="Database connection failed"
         data-amb-alert-btn="Try Again">
    </div>
</div>
```

#### Multiple Alerts with RTL

```html
<div class="amb-alert-container" data-amb-alert-step="500">
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="خطای اعتبارسنجی"
         data-amb-alert-msg="نام کاربری الزامی است"
         data-amb-alert-rtl="true"
         data-amb-alert-btn="باشه">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="موفقیت"
         data-amb-alert-msg="اطلاعات با موفقیت ذخیره شد"
         data-amb-alert-rtl="true"
         data-amb-alert-theme="dark"
         data-amb-alert-btn="بستن">
    </div>
</div>
```

#### Form Validation Error Sequence

```html
<div class="amb-alert-container" data-amb-alert-step="400">
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="Validation Error 1"
         data-amb-alert-msg="Please enter your full name"
         data-amb-alert-size="sm">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="Validation Error 2"
         data-amb-alert-msg="Email address format is invalid"
         data-amb-alert-size="sm">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="Validation Error 3"
         data-amb-alert-msg="Password must contain at least one number"
         data-amb-alert-size="sm">
    </div>
</div>
```

#### Success and Info Sequence

```html
<div class="amb-alert-container" data-amb-alert-step="600">
    <!-- Success alert with auto-close -->
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="Upload Complete"
         data-amb-alert-msg="Your file has been uploaded successfully"
         data-amb-alert-auto-close="2000">
    </div>
    
    <!-- Info alert with dark theme -->
    <div class="amb-alert-item" 
         data-amb-alert-type="info"
         data-amb-alert-title="New Version"
         data-amb-alert-msg="Version 2.0 is now available for download"
         data-amb-alert-theme="dark"
         data-amb-alert-btn="Download Now">
    </div>
    
    <!-- Warning alert -->
    <div class="amb-alert-item" 
         data-amb-alert-type="warning"
         data-amb-alert-title="Session Expiring"
         data-amb-alert-msg="Your session will expire in 5 minutes"
         data-amb-alert-btn="Extend Session">
    </div>
</div>
```

---

## Use Cases

### Form Validation Errors

```html
<!-- PHP generated validation errors -->
<div class="amb-toast-container" data-amb-toast-step="300">
    <?php foreach ($validation_errors as $error): ?>
        <div class="amb-toast-item" 
             data-amb-toast-type="error" 
             data-amb-toast-msg="<?php echo htmlspecialchars($error); ?>">
        </div>
    <?php endforeach; ?>
</div>
```

### Success Messages After Form Submission

```html
<div class="amb-alert-container" data-amb-alert-step="400">
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="Registration Complete"
         data-amb-alert-msg="Welcome! Please check your email for verification"
         data-amb-alert-btn="Continue">
    </div>
</div>
```

### System Notifications Queue

```html
<div class="amb-toast-container" data-amb-toast-step="200">
    <div class="amb-toast-item" data-amb-toast-type="info" data-amb-toast-msg="System update scheduled for tonight at 2 AM"></div>
    <div class="amb-toast-item" data-amb-toast-type="warning" data-amb-toast-msg="Backup reminder: Please backup your data"></div>
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="New features available! Click to learn more"></div>
</div>
```

### Multi-step Process Feedback

```html
<div class="amb-alert-container" data-amb-alert-step="300">
    <div class="amb-alert-item" 
         data-amb-alert-type="info"
         data-amb-alert-title="Step 1 of 3"
         data-amb-alert-msg="Analyzing your data..."
         data-amb-alert-auto-close="1500">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="info"
         data-amb-alert-title="Step 2 of 3"
         data-amb-alert-msg="Processing results..."
         data-amb-alert-auto-close="1500">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="Complete!"
         data-amb-alert-msg="Process finished successfully"
         data-amb-alert-auto-close="2000">
    </div>
</div>
```

---

## Server-Side Integration

### PHP Example

```php
<?php
// Collect messages from backend
$success_messages = [];
$error_messages = [];
$warning_messages = [];

// Example: Form processing
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $name = $_POST['name'] ?? '';
    $email = $_POST['email'] ?? '';
    
    if (empty($name)) {
        $error_messages[] = 'Name is required';
    }
    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        $error_messages[] = 'Valid email is required';
    }
    
    if (empty($error_messages)) {
        $success_messages[] = 'Form submitted successfully!';
        $success_messages[] = 'Thank you for your submission';
    }
}

// Display error toasts if any
if (!empty($error_messages)): ?>
    <div class="amb-toast-container" data-amb-toast-step="300">
        <?php foreach ($error_messages as $error): ?>
            <div class="amb-toast-item" 
                 data-amb-toast-type="error" 
                 data-amb-toast-msg="<?php echo htmlspecialchars($error); ?>">
            </div>
        <?php endforeach; ?>
    </div>
<?php endif; ?>

<!-- Display success alerts if any -->
<?php if (!empty($success_messages)): ?>
    <div class="amb-alert-container" data-amb-alert-step="400">
        <?php foreach ($success_messages as $success): ?>
            <div class="amb-alert-item" 
                 data-amb-alert-type="success"
                 data-amb-alert-title="Success!"
                 data-amb-alert-msg="<?php echo htmlspecialchars($success); ?>"
                 data-amb-alert-auto-close="2000">
            </div>
        <?php endforeach; ?>
    </div>
<?php endif; ?>
```

### ASP.NET C# Example

```csharp
@{
    var errors = new List<string>();
    var successes = new List<string>();
    
    if (IsPost)
    {
        var name = Request["name"];
        if (string.IsNullOrEmpty(name))
        {
            errors.Add("Name is required");
        }
        
        if (errors.Count == 0)
        {
            successes.Add("Profile updated successfully");
            successes.Add("Redirecting to dashboard...");
        }
    }
}

@if (errors.Any())
{
    <div class="amb-toast-container" data-amb-toast-step="300">
        @foreach (var error in errors)
        {
            <div class="amb-toast-item" 
                 data-amb-toast-type="error" 
                 data-amb-toast-msg="@error">
            </div>
        }
    </div>
}

@if (successes.Any())
{
    <div class="amb-alert-container" data-amb-alert-step="500">
        @foreach (var success in successes)
        {
            <div class="amb-alert-item" 
                 data-amb-alert-type="success"
                 data-amb-alert-title="Success"
                 data-amb-alert-msg="@success"
                 data-amb-alert-auto-close="2000">
            </div>
        }
    </div>
}
```

### Node.js / Express Example

```javascript
// In your route handler
app.post('/submit', (req, res) => {
    const errors = [];
    const successes = [];
    
    if (!req.body.email) {
        errors.push('Email is required');
        errors.push('Please provide a valid email address');
    }
    
    if (errors.length === 0) {
        successes.push('Form submitted successfully');
        successes.push('Check your email for confirmation');
    }
    
    res.render('result', { 
        toastErrors: errors,
        alertSuccesses: successes 
    });
});
```

```ejs
<!-- In your template -->
<% if (toastErrors && toastErrors.length) { %>
    <div class="amb-toast-container" data-amb-toast-step="300">
        <% toastErrors.forEach(error => { %>
            <div class="amb-toast-item" 
                 data-amb-toast-type="error" 
                 data-amb-toast-msg="<%= error %>">
            </div>
        <% }); %>
    </div>
<% } %>

<% if (alertSuccesses && alertSuccesses.length) { %>
    <div class="amb-alert-container" data-amb-alert-step="400">
        <% alertSuccesses.forEach(success => { %>
            <div class="amb-alert-item" 
                 data-amb-alert-type="success"
                 data-amb-alert-title="Success!"
                 data-amb-alert-msg="<%= success %>"
                 data-amb-alert-auto-close="2000">
            </div>
        <% }); %>
    </div>
<% } %>
```

---

## Best Practices

### 1. Set Appropriate Delays

```html
<!-- For toasts - shorter delays -->
<div class="amb-toast-container" data-amb-toast-step="300">

<!-- For modals - give users time to read -->
<div class="amb-alert-container" data-amb-alert-step="600">
```

### 2. Use Auto-Close for Non-Critical Alerts

```html
<!-- Non-critical - auto close -->
<div class="amb-alert-item" 
     data-amb-alert-type="success"
     data-amb-alert-msg="Operation completed"
     data-amb-alert-auto-close="2000">

<!-- Critical - require user action -->
<div class="amb-alert-item" 
     data-amb-alert-type="error"
     data-amb-alert-msg="System failure"
     data-amb-alert-btn="Contact Support">
```

### 3. Combine Toast and Alert Containers

```html
<!-- Show toasts for minor issues -->
<div class="amb-toast-container" data-amb-toast-step="200">
    <!-- Minor validation warnings -->
</div>

<!-- Show modals for critical errors -->
<div class="amb-alert-container" data-amb-alert-step="500">
    <!-- Critical system errors -->
</div>
```

### 4. Use RTL for Persian/Arabic Content

```html
<div class="amb-alert-item" 
     data-amb-alert-type="error"
     data-amb-alert-title="خطا"
     data-amb-alert-msg="متن فارسی"
     data-amb-alert-rtl="true"
     data-amb-alert-btn="باشه">
</div>
```

### 5. Provide Contextual Titles

```html
<!-- Good -->
data-amb-alert-title="Validation Error"

<!-- Better -->
data-amb-alert-title="خطا در فیلد ایمیل"
```

---

## Complete Working Example

```html
<!DOCTYPE html>
<html lang="en" dir="ltr">
<head>
    <meta charset="UTF-8">
    <title>Auto Messages Demo</title>
    <link rel="stylesheet" href="ambmsg.css">
    <style>
        body { padding: 2rem; font-family: system-ui; }
        button { padding: 10px 20px; margin: 10px; cursor: pointer; }
    </style>
</head>
<body>

<h1>🤖 Auto Message System Demo</h1>
<p>This page demonstrates automatic toast and alert queues using HTML attributes only.</p>

<!-- Toast Container Example -->
<h2>📢 Toast Notifications</h2>
<div class="amb-toast-container" data-amb-toast-step="400">
    <div class="amb-toast-item" data-amb-toast-type="success" data-amb-toast-msg="✅ File uploaded successfully"></div>
    <div class="amb-toast-item" data-amb-toast-type="error" data-amb-toast-msg="❌ Connection timeout"></div>
    <div class="amb-toast-item" data-amb-toast-type="warning" data-amb-toast-msg="⚠️ Disk space low"></div>
    <div class="amb-toast-item" data-amb-toast-type="info" data-amb-toast-msg="ℹ️ New update available"></div>
</div>

<!-- Alert Container Example -->
<h2>🔔 Modal Alerts</h2>
<div class="amb-alert-container" data-amb-alert-step="600">
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="System Error"
         data-amb-alert-msg="Database connection failed. Please try again later."
         data-amb-alert-btn="Retry">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="Operation Complete"
         data-amb-alert-msg="Your changes have been saved successfully"
         data-amb-alert-auto-close="2500">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="warning"
         data-amb-alert-title="Session Expiring"
         data-amb-alert-msg="Your session will expire in 5 minutes"
         data-amb-alert-theme="dark"
         data-amb-alert-btn="Extend">
    </div>
</div>

<!-- RTL Example -->
<h2>🌐 RTL Messages (Persian)</h2>
<div class="amb-alert-container" data-amb-alert-step="500">
    <div class="amb-alert-item" 
         data-amb-alert-type="error"
         data-amb-alert-title="خطای اعتبارسنجی"
         data-amb-alert-msg="لطفاً فیلدهای required را پر کنید"
         data-amb-alert-rtl="true"
         data-amb-alert-btn="باشه">
    </div>
    
    <div class="amb-alert-item" 
         data-amb-alert-type="success"
         data-amb-alert-title="موفقیت"
         data-amb-alert-msg="اطلاعات با موفقیت ذخیره شد"
         data-amb-alert-rtl="true"
         data-amb-alert-theme="dark"
         data-amb-alert-btn="بستن">
    </div>
</div>

<script src="ambmsg.js"></script>

<script>
    // Optional: Manual triggers for demonstration
    console.log('Auto containers will process automatically on page load');
</script>

</body>
</html>
```

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Messages not appearing | Ensure containers have correct classes: `amb-toast-container` or `amb-alert-container` |
| Delays not working | Check `data-amb-toast-step` or `data-amb-alert-step` values are numbers |
| RTL not applying | Add `data-amb-alert-rtl="true"` to each alert item |
| Theme not applying | Add `data-amb-alert-theme="dark"` to alert items |
| Auto-close not working | Ensure `data-amb-alert-auto-close` is a valid number (ms) |
| Messages showing out of order | Increase step delay value |
| Toast not styled | Verify CSS is loaded |
| Modal not showing | Check console for JavaScript errors |

---

## Key Benefits

✅ **No JavaScript Required** - Pure HTML configuration  
✅ **Server-Side Friendly** - Easy to generate with backend languages  
✅ **Sequential Processing** - Messages appear one after another  
✅ **Configurable Delays** - Control timing between messages  
✅ **Multiple Types** - Success, error, warning, info  
✅ **RTL Support** - Perfect for Persian/Arabic content  
✅ **Theme Support** - Light and dark modes  
✅ **Auto Cleanup** - Containers removed after processing  

---

## Next Steps

- Try the [JavaScript API](02-javascript-usage.md) for dynamic control
- See [HTML Attribute Usage](03-html-attribute-usage.md) for manual modals
- Explore the [Complete API Reference](01-complete-api-documentation.md) for all options