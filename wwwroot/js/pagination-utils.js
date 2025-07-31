/**
 * Pagination and Loading Utilities for RazorTableDemo
 * This file contains reusable JavaScript functions for pagination, loading states, and UI interactions
 */

// Utility functions for loading states
const LoadingManager = {
    showPageLoading() {
        const overlay = document.getElementById('pageLoadingOverlay');
        if (overlay) {
            overlay.style.display = 'flex';
        }
    },
    
    hidePageLoading() {
        const overlay = document.getElementById('pageLoadingOverlay');
        if (overlay) {
            overlay.style.display = 'none';
        }
    },
    
    showButtonLoading(buttonId) {
        const button = document.getElementById(buttonId);
        if (button) {
            button.disabled = true;
            const buttonText = button.querySelector('.button-text');
            const buttonLoading = button.querySelector('.button-loading');
            
            if (buttonText) buttonText.style.display = 'none';
            if (buttonLoading) buttonLoading.style.display = 'inline-block';
        }
    },
    
    hideButtonLoading(buttonId) {
        const button = document.getElementById(buttonId);
        if (button) {
            button.disabled = false;
            const buttonText = button.querySelector('.button-text');
            const buttonLoading = button.querySelector('.button-loading');
            
            if (buttonText) buttonText.style.display = 'inline-block';
            if (buttonLoading) buttonLoading.style.display = 'none';
        }
    },
    

    
    showTableLoading() {
        const tableContainer = document.getElementById('tableContainer');
        const tableLoading = document.getElementById('tableLoading');
        const emptyState = document.getElementById('emptyState');
        
        if (tableContainer) tableContainer.style.display = 'none';
        if (tableLoading) tableLoading.style.display = 'block';
        if (emptyState) emptyState.style.display = 'none';
    },
    
    hideTableLoading() {
        const tableLoading = document.getElementById('tableLoading');
        const tableContainer = document.getElementById('tableContainer');
        
        if (tableLoading) tableLoading.style.display = 'none';
        if (tableContainer) tableContainer.style.display = 'block';
    },
    
    showEmptyState() {
        const tableContainer = document.getElementById('tableContainer');
        const tableLoading = document.getElementById('tableLoading');
        const emptyState = document.getElementById('emptyState');
        
        if (tableContainer) tableContainer.style.display = 'none';
        if (tableLoading) tableLoading.style.display = 'none';
        if (emptyState) emptyState.style.display = 'block';
    },
    

};

// Page size selector functionality
const PageSizeManager = {
    initialize() {
        const pageSizeSelector = document.getElementById('pageSizeSelector');
        if (pageSizeSelector) {
            // Remove any existing event listeners
            pageSizeSelector.removeEventListener('change', PageSizeManager.handlePageSizeChange);
            
            // Add new event listener
            pageSizeSelector.addEventListener('change', PageSizeManager.handlePageSizeChange);
            console.log('Page size selector initialized');
        } else {
            console.log('Page size selector not found');
        }
    },
    
    handlePageSizeChange(event) {
        const pageSize = event.target.value;
        console.log('Page size changed to:', pageSize);
        
        // Get current URL parameters to preserve existing filters
        const urlParams = new URLSearchParams(window.location.search);
        const clientCode = urlParams.get('ClientCode') || '';
        const authorityKey = urlParams.get('AuthorityKey') || '';
        
        // Build URL with current search parameters
        let url = '?PageSize=' + pageSize + '&Page=1'; // Reset to first page when changing page size
        
        if (clientCode && clientCode.trim() !== '') {
            url += '&ClientCode=' + encodeURIComponent(clientCode.trim());
        }
        if (authorityKey && authorityKey.trim() !== '') {
            url += '&AuthorityKey=' + encodeURIComponent(authorityKey.trim());
        }
        
        console.log('Current filters - ClientCode:', clientCode, 'AuthorityKey:', authorityKey);
        console.log('Navigating to:', url);
        window.location.href = url;
    },
    

};

// Form and button event handlers
const EventHandlers = {
    initializeSearchForm() {
        const searchForm = document.getElementById('searchForm');
        if (searchForm) {
            searchForm.addEventListener('submit', function(e) {
                LoadingManager.showButtonLoading('searchButton');
                
                // Simulate some delay to show the loading state
                setTimeout(() => {
                    LoadingManager.hideButtonLoading('searchButton');
                }, 1000);
            });
        }
    },
    
    initializeRefreshButton() {
        const refreshButton = document.getElementById('refreshButton');
        if (refreshButton) {
            refreshButton.addEventListener('click', function() {
                LoadingManager.showButtonLoading('refreshButton');
                
                // Reload the page
                setTimeout(() => {
                    window.location.reload();
                }, 500);
            });
        }
    },
    
    initializeExportButton() {
        const exportButton = document.getElementById('exportButton');
        if (exportButton) {
            exportButton.addEventListener('click', function() {
                LoadingManager.showButtonLoading('exportButton');
                
                // Simulate export process
                setTimeout(() => {
                    LoadingManager.hideButtonLoading('exportButton');
                    alert('Export functionality would be implemented here');
                }, 2000);
            });
        }
    },
    
    initializeBackButton() {
        const backButton = document.getElementById('backButton');
        if (backButton) {
            backButton.addEventListener('click', function() {
                LoadingManager.showPageLoading();
                
                setTimeout(() => {
                    window.history.back();
                }, 500);
            });
        }
    },
    

};



// Main initialization function
function initializePage() {
    // Hide any initial loading states
    LoadingManager.hidePageLoading();
    
    // Initialize all components
    PageSizeManager.initialize();
    EventHandlers.initializeSearchForm();
    EventHandlers.initializeRefreshButton();
    EventHandlers.initializeExportButton();
    EventHandlers.initializeBackButton();
    
    console.log('Page utilities initialized successfully');
}

// Export functions for use in other pages
window.PaginationUtils = {
    LoadingManager,
    PageSizeManager,
    EventHandlers,
    initializePage
};

// Auto-initialize when DOM is ready
document.addEventListener('DOMContentLoaded', initializePage); 