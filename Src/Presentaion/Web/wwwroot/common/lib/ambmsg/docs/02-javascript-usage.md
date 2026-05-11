# 🟨 JavaScript Usage Documentation

> Complete guide for using AmbMsg programmatically with JavaScript

---

## 📑 Table of Contents

1. [Installation & Setup](#installation--setup)
2. [Modal API](#modal-api)
   - [Creating Modals](#creating-modals)
   - [Opening Modals](#opening-modals)
   - [Managing Modals](#managing-modals)
   - [Modal Configuration](#modal-configuration)
3. [Toast API](#toast-api)
4. [Alert API](#alert-api)
5. [Confirm API](#confirm-api)
6. [Advanced Usage](#advanced-usage)
7. [Practical Examples](#practical-examples)

---

## Installation & Setup

```html
<!-- Include CSS -->
<link rel="stylesheet" href="ambmsg.css">

<!-- Include JavaScript -->
<script src="ambmsg.js"></script>

<script>
    const Amb = window.AmbMsg;
    // Global shortcuts: AlertSuccess, ToastError, etc. are also available
</script>
```

---

## Modal API

### Creating Modals

```javascript
// Create modal (hidden)
const modal = AmbMsg.create('my-modal', {
    title: 'My Modal',
    body: '<p>Modal content here</p>',
    footer: '<button class="amb-btn amb-btn-primary" data-amb-dismiss="modal">Close</button>'
});
modal.show();

// Create and show immediately
AmbMsg.open('another-modal', {
    title: 'Auto-open Modal',
    body: '<p>This modal opens immediately</p>'
});
```

### Managing Modals

```javascript
// Get modal instance
const modal = AmbMsg.Modal.getInstance('my-modal');
// or
const modal = AmbMsg.Modal.getOrCreateInstance('my-modal', options);

// Control modal
modal.show();           // Show modal
modal.hide();           // Hide modal
modal.toggle();         // Toggle visibility
modal.dispose();        // Destroy modal completely
modal.setLoading(true); // Show loading state

// Check visibility
if (modal.isVisible) {
    console.log('Modal is visible');
}

// Close by ID
AmbMsg.close('my-modal');
AmbMsg.destroy('my-modal');

// Set global defaults
AmbMsg.config({
    size: 'lg',
    position: 'top-center',
    animOpen: 'bounce'
});
```

---

## Toast API

```javascript
// Basic toast
AmbMsg.AmbToast.show(message, type, duration);

// Type-specific methods
AmbMsg.AmbToast.success('File saved!', 3000);
AmbMsg.AmbToast.error('Connection failed!', 4000);
AmbMsg.AmbToast.warning('Disk space low', 2000);
AmbMsg.AmbToast.info('New update available', 5000);

// Global shortcuts
ToastSuccess('Operation completed!');
ToastError('Something went wrong!');
ToastWarning('Please check your input!');
ToastInfo('New message received');
FireToast('success', 'Dynamic toast!', 3000);
```

---

## Alert API

### Basic Alerts

```javascript
// Single message alert
AmbMsg.alert({
    type: 'success',
    message: 'Operation completed successfully!',
    title: 'Success',
    btnText: 'OK',
    autoClose: 2000
});

// Type shortcuts
AmbMsg.success('Success!', 'Your profile has been updated');
AmbMsg.error('Error!', 'Unable to save changes');
AmbMsg.warning('Warning!', 'Please review your input');
AmbMsg.info('Info', 'New version available');

// Global shortcuts
AlertSuccess('Operation completed!');
AlertError('خطا!', 'مشکلی پیش آمده', { rtl: true });
AlertWarning('Warning!', 'Session expiring');
AlertInfo('Info', 'New features available');
```

### Message List Alerts

```javascript
// Multiple messages
AmbMsg.error('Validation Errors', [
    'Username is required',
    'Email must be valid',
    'Password must be at least 8 characters'
], {
    theme: 'dark',
    btnText: 'Fix Errors',
    rtl: true
});

// Using shortcuts with arrays
AlertError('خطاهای فرم', [
    'نام کاربری الزامی است',
    'ایمیل نامعتبر است'
], { btnText: 'باشه' });
```

---

## Confirm API

```javascript
AmbMsg.confirm({
    title: 'Delete Item',
    message: 'Are you sure you want to delete this item?',
    confirmText: 'Yes, Delete',
    cancelText: 'Cancel',
    size: 'sm',
    onConfirm: () => {
        console.log('Item deleted');
        ToastSuccess('Item deleted successfully');
    },
    onCancel: () => {
        console.log('Action cancelled');
    }
});
```

---

## Advanced Usage

### Dynamic Content Updates

```javascript
const modal = AmbMsg.open('dynamic-modal', {
    title: 'Loading...',
    body: '<div>Fetching data...</div>'
});

// Update content after API call
setTimeout(() => {
    modal.element.querySelector('.amb-title').innerHTML = 'User Profile';
    modal._body.innerHTML = `
        <h3>John Doe</h3>
        <p>Email: john@example.com</p>
    `;
}, 2000);
```

### Loading States

```javascript
const modal = AmbMsg.open('loading-demo', {
    title: 'Processing',
    body: '<p>Please wait...</p>',
    backdrop: 'static'
});

modal.setLoading(true);

setTimeout(() => {
    modal.setLoading(false);
    modal._body.innerHTML = '<p>✅ Data loaded successfully!</p>';
    setTimeout(() => modal.hide(), 1500);
}, 3000);
```

### Event Handling

```javascript
const modal = AmbMsg.open('event-modal', { title: 'Event Demo', body: '<p>Content</p>' });
const wrapper = modal.element;

wrapper.addEventListener('show.amb.modal', (e) => {
    console.log('Opening...');
});

wrapper.addEventListener('shown.amb.modal', (e) => {
    console.log('Fully opened');
});

wrapper.addEventListener('hide.amb.modal', (e) => {
    if (!confirm('Close?')) e.preventDefault();
});

wrapper.addEventListener('hidden.amb.modal', (e) => {
    console.log('Closed');
});
```

### Multiple Modals

```javascript
// Modals stack automatically with proper z-index
AmbMsg.open('modal1', { title: 'First Modal', size: 'md' });
setTimeout(() => {
    AmbMsg.open('modal2', { title: 'Second Modal', size: 'sm', position: 'top-right' });
}, 1000);
```

---

## Practical Examples

### Registration Form

```javascript
function openRegisterForm() {
    AmbMsg.open('register-form', {
        title: 'Registration Form',
        body: `
            <form id="registerForm">
                <label>Name: <input type="text" id="regName" required></label><br><br>
                <label>Email: <input type="email" id="regEmail" required></label><br><br>
                <label>Password: <input type="password" id="regPass" required></label>
            </form>
        `,
        footer: `
            <button class="amb-btn amb-btn-outline" data-amb-dismiss="modal">Cancel</button>
            <button class="amb-btn amb-btn-primary" id="submitReg">Register</button>
        `,
        size: 'md',
        backdrop: 'static',
        afterload: () => {
            document.getElementById('submitReg').onclick = () => {
                const name = document.getElementById('regName').value;
                if (!name) {
                    AmbMsg.alert({ message: 'Name is required!', type: 'error' });
                    return;
                }
                AmbMsg.success('Success!', `${name}, you have been registered!`);
                AmbMsg.close('register-form');
            };
        }
    });
}
```

### Data Submission with Loading

```javascript
async function submitData() {
    const modal = AmbMsg.open('submit-modal', {
        title: 'Submitting Data',
        body: '<p>Please wait...</p>',
        backdrop: 'static',
        keyboard: false
    });
    
    modal.setLoading(true);
    
    try {
        const response = await fetch('/api/data', { method: 'POST' });
        const result = await response.json();
        
        modal.setLoading(false);
        modal._body.innerHTML = '<p>✅ Data submitted successfully!</p>';
        setTimeout(() => modal.hide(), 2000);
    } catch (error) {
        modal.setLoading(false);
        AmbMsg.error('Error', 'Failed to submit data');
    }
}
```

### Wizard/Step-by-Step

```javascript
let currentStep = 1;

function showWizardStep(step) {
    const steps = {
        1: { title: 'Step 1: Personal Info', body: '<label>Name: <input type="text" id="step1Name"></label>' },
        2: { title: 'Step 2: Address', body: '<label>Address: <textarea id="step2Addr"></textarea></label>' },
        3: { title: 'Step 3: Confirm', body: '<p>Review your information</p>' }
    };
    
    const data = steps[step];
    AmbMsg.open('wizard', {
        title: data.title,
        body: data.body,
        footer: `
            ${step > 1 ? '<button class="amb-btn amb-btn-outline" id="prevStep">← Previous</button>' : ''}
            ${step < 3 ? '<button class="amb-btn amb-btn-primary" id="nextStep">Next →</button>' : '<button class="amb-btn amb-btn-success" data-amb-dismiss="modal">Finish</button>'}
        `,
        size: 'md',
        backdrop: 'static',
        afterload: () => {
            if (step < 3) {
                document.getElementById('nextStep')?.addEventListener('click', () => {
                    currentStep++;
                    AmbMsg.close('wizard');
                    setTimeout(() => showWizardStep(currentStep), 300);
                });
            }
            if (step > 1) {
                document.getElementById('prevStep')?.addEventListener('click', () => {
                    currentStep--;
                    AmbMsg.close('wizard');
                    setTimeout(() => showWizardStep(currentStep), 300);
                });
            }
        }
    });
}
```

---

## Best Practices

1. **Always use unique IDs** for each modal
2. **Clean up modals** with `dispose()` when no longer needed
3. **Use `backdrop: 'static'`** for important forms to prevent accidental closure
4. **Provide loading states** for async operations
5. **Handle errors gracefully** with error alerts
6. **Use event listeners** for complex interactions
7. **Set global defaults** with `AmbMsg.config()` for consistency

---

## Complete Example

```javascript
// Initialize with defaults
AmbMsg.config({
    size: 'md',
    animOpen: 'slide-up',
    animClose: 'slide-down',
    theme: 'dark'
});

// Complete user interaction flow
async function completeUserFlow() {
    // Confirm action
    AmbMsg.confirm({
        title: 'Start Process',
        message: 'Ready to begin?',
        onConfirm: async () => {
            // Show loading
            const modal = AmbMsg.open('process-modal', {
                title: 'Processing',
                body: '<p>Working...</p>',
                backdrop: 'static'
            });
            modal.setLoading(true);
            
            // Simulate work
            await new Promise(resolve => setTimeout(resolve, 2000));
            
            // Show success
            modal.setLoading(false);
            modal._body.innerHTML = '<p>✅ Process completed!</p>';
            setTimeout(() => modal.hide(), 1500);
            
            // Show final alert
            setTimeout(() => {
                AmbMsg.success('Complete!', 'Process finished successfully');
            }, 2000);
        }
    });
}
```

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Modal doesn't appear | Check console for errors, ensure CSS is loaded |
| Events not firing | Verify event name includes `.amb.modal` suffix |
| Loading spinner not showing | Call `setLoading(true)` after modal is open |
| Multiple modals interfere | Each modal needs a unique ID |
| Toast not showing | Check if `AmbMsg.AmbToast` is accessible |