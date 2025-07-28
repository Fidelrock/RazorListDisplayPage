# Busy Indicators in Razor Pages

This document provides a comprehensive guide to implementing various types of busy indicators (loading states) in Razor Pages applications.

## Table of Contents

1. [Types of Busy Indicators](#types-of-busy-indicators)
2. [Implementation Methods](#implementation-methods)
3. [Best Practices](#best-practices)
4. [Examples](#examples)
5. [Reusable Components](#reusable-components)

## Types of Busy Indicators

### 1. **Button Loading States**
- Spinners within buttons
- Text changes during loading
- Button disabled state
- Inline animations (dots, pulse)

### 2. **Page Loading Overlays**
- Full-screen loading overlays
- Modal-style loading dialogs
- Background blur effects

### 3. **Progress Bars**
- Determinate progress (with percentages)
- Indeterminate progress (animated)
- Striped and animated progress bars

### 4. **Content Loading States**
- Skeleton screens
- Loading placeholders
- Table loading states
- Form loading states

### 5. **AJAX Loading Indicators**
- Request/response indicators
- Error handling with loading states
- Success/failure feedback

## Implementation Methods

### Method 1: CSS-Based Loading States

```css
/* Button loading state */
.btn-loading {
    position: relative;
    pointer-events: none;
}

.btn-loading::after {
    content: '';
    position: absolute;
    width: 16px;
    height: 16px;
    margin: auto;
    border: 2px solid transparent;
    border-top-color: #ffffff;
    border-radius: 50%;
    animation: button-loading-spinner 1s ease infinite;
}

@keyframes button-loading-spinner {
    from {
        transform: rotate(0turn);
    }
    to {
        transform: rotate(1turn);
    }
}
```

### Method 2: JavaScript-Based Loading States

```javascript
const LoadingManager = {
    showButtonLoading(buttonId) {
        const button = document.getElementById(buttonId);
        if (button) {
            button.disabled = true;
            button.querySelector('.button-text').style.display = 'none';
            button.querySelector('.button-loading').style.display = 'inline-block';
        }
    },
    
    hideButtonLoading(buttonId) {
        const button = document.getElementById(buttonId);
        if (button) {
            button.disabled = false;
            button.querySelector('.button-text').style.display = 'inline-block';
            button.querySelector('.button-loading').style.display = 'none';
        }
    }
};
```

### Method 3: Bootstrap-Based Loading States

```html
<!-- Button with spinner -->
<button class="btn btn-primary" id="loadingButton">
    <span class="button-text">Submit</span>
    <span class="button-loading" style="display: none;">
        <span class="spinner-border spinner-border-sm me-2"></span>
        Loading...
    </span>
</button>

<!-- Progress bar -->
<div class="progress">
    <div id="progressBar" class="progress-bar progress-bar-striped progress-bar-animated" 
         role="progressbar" style="width: 0%"></div>
</div>
```

## Best Practices

### 1. **User Experience**
- Always provide visual feedback for user actions
- Use appropriate loading times (not too fast, not too slow)
- Include descriptive text with loading indicators
- Provide fallback states for errors

### 2. **Performance**
- Use CSS animations instead of JavaScript when possible
- Minimize DOM manipulation during loading states
- Use requestAnimationFrame for smooth animations
- Implement proper cleanup for loading states

### 3. **Accessibility**
- Include proper ARIA labels
- Use `role="status"` for loading indicators
- Provide screen reader support
- Ensure keyboard navigation works during loading

### 4. **Consistency**
- Use consistent loading patterns across your application
- Maintain consistent colors and animations
- Follow your design system guidelines
- Use appropriate loading indicators for different contexts

## Examples

### Example 1: Form Submission with Loading State

```html
<form id="submitForm">
    <input type="text" name="name" required>
    <button type="submit" id="submitButton">
        <span class="button-text">Submit Form</span>
        <span class="button-loading" style="display: none;">
            <span class="spinner-border spinner-border-sm me-2"></span>
            Submitting...
        </span>
    </button>
</form>

<script>
document.getElementById('submitForm').addEventListener('submit', function(e) {
    e.preventDefault();
    
    // Show loading state
    LoadingManager.showButtonLoading('submitButton');
    
    // Simulate form submission
    setTimeout(() => {
        LoadingManager.hideButtonLoading('submitButton');
        alert('Form submitted successfully!');
    }, 2000);
});
</script>
```

### Example 2: AJAX Data Loading

```html
<div id="dataContainer">
    <table id="dataTable" class="table">
        <tbody id="tableBody">
            <!-- Data will be loaded here -->
        </tbody>
    </table>
</div>

<div id="loadingState" class="text-center py-4" style="display: none;">
    <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
    </div>
    <p class="mt-2">Loading data...</p>
</div>

<script>
function loadData() {
    // Show loading state
    document.getElementById('dataContainer').style.display = 'none';
    document.getElementById('loadingState').style.display = 'block';
    
    // Make AJAX request
    fetch('/api/data')
        .then(response => response.json())
        .then(data => {
            // Hide loading state
            document.getElementById('loadingState').style.display = 'none';
            document.getElementById('dataContainer').style.display = 'block';
            
            // Update table
            updateTable(data);
        })
        .catch(error => {
            // Handle error
            document.getElementById('loadingState').style.display = 'none';
            document.getElementById('dataContainer').style.display = 'block';
            alert('Error loading data: ' + error.message);
        });
}
</script>
```

### Example 3: Page Loading Overlay

```html
<!-- Page loading overlay -->
<div id="pageLoadingOverlay" class="loading-overlay" style="display: none;">
    <div class="loading-content">
        <div class="spinner-border text-white" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <p class="mt-3 text-white">Loading page...</p>
    </div>
</div>

<style>
.loading-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.8);
    z-index: 9999;
    display: flex;
    justify-content: center;
    align-items: center;
}

.loading-content {
    text-align: center;
    background: rgba(0, 0, 0, 0.9);
    padding: 2rem;
    border-radius: 10px;
    color: white;
}
</style>

<script>
function showPageLoading() {
    document.getElementById('pageLoadingOverlay').style.display = 'flex';
}

function hidePageLoading() {
    document.getElementById('pageLoadingOverlay').style.display = 'none';
}
</script>
```

## Reusable Components

### Loading Partial View

Create a reusable partial view (`_LoadingPartial.cshtml`):

```html
@{
    var loadingId = ViewData["LoadingId"] ?? "loading";
    var loadingText = ViewData["LoadingText"] ?? "Loading...";
    var loadingType = ViewData["LoadingType"] ?? "spinner";
    var loadingSize = ViewData["LoadingSize"] ?? "normal";
}

<div id="@loadingId" class="loading-@loadingType" style="display: none;">
    <!-- Loading content based on type -->
</div>
```

### Usage in Razor Pages

```html
@await Html.PartialAsync("_LoadingPartial", new { 
    LoadingId = "myLoading", 
    LoadingText = "Loading data...", 
    LoadingType = "spinner" 
})
```

### JavaScript Loading Manager

```javascript
window.LoadingManager = {
    show(loadingId = 'loading') {
        const loadingElement = document.getElementById(loadingId);
        if (loadingElement) {
            loadingElement.style.display = 'flex';
        }
    },
    
    hide(loadingId = 'loading') {
        const loadingElement = document.getElementById(loadingId);
        if (loadingElement) {
            loadingElement.style.display = 'none';
        }
    },
    
    showWithText(loadingId, text) {
        const loadingElement = document.getElementById(loadingId);
        if (loadingElement) {
            const textElement = loadingElement.querySelector('p');
            if (textElement) {
                textElement.textContent = text;
            }
            loadingElement.style.display = 'flex';
        }
    }
};
```

## Integration with Razor Pages

### Server-Side Loading States

```csharp
public class MyPageModel : PageModel
{
    public bool IsLoading { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        IsLoading = true;
        
        try
        {
            // Perform some async operation
            await Task.Delay(2000); // Simulate work
            
            return RedirectToPage();
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

### Client-Side Integration

```html
@if (Model.IsLoading)
{
    <div class="loading-overlay">
        <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>
}
```

## Testing Loading States

### Unit Testing

```csharp
[Fact]
public async Task OnPostAsync_ShowsLoadingState()
{
    // Arrange
    var pageModel = new MyPageModel();
    
    // Act
    var result = await pageModel.OnPostAsync();
    
    // Assert
    Assert.True(pageModel.IsLoading);
}
```

### Integration Testing

```csharp
[Fact]
public async Task LoadingIndicator_AppearsDuringFormSubmission()
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.PostAsync("/MyPage", new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("name", "test")
    }));
    
    // Assert
    response.EnsureSuccessStatusCode();
    // Add assertions for loading state
}
```

## Conclusion

Busy indicators are essential for providing good user experience in Razor Pages applications. By implementing the patterns and examples shown in this document, you can create responsive and user-friendly loading states that enhance the overall user experience.

Remember to:
- Choose appropriate loading indicators for different contexts
- Maintain consistency across your application
- Consider accessibility requirements
- Test loading states thoroughly
- Use reusable components when possible

For more examples and demonstrations, visit the Loading Demo page in your application. 