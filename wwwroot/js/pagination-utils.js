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
    
    showProgressBar() {
        const progressContainer = document.getElementById('progressContainer');
        const progressBar = document.getElementById('progressBar');
        if (progressContainer) {
            progressContainer.style.display = 'block';
        }
        if (progressBar) {
            progressBar.style.width = '0%';
        }
    },
    
    updateProgress(percentage) {
        const progressBar = document.getElementById('progressBar');
        if (progressBar) {
            progressBar.style.width = percentage + '%';
        }
    },
    
    hideProgressBar() {
        const progressContainer = document.getElementById('progressContainer');
        if (progressContainer) {
            progressContainer.style.display = 'none';
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
    
    showError(message) {
        const errorAlert = document.getElementById('errorAlert');
        const errorMessage = document.getElementById('errorMessage');
        
        if (errorMessage) errorMessage.textContent = message;
        if (errorAlert) {
            errorAlert.style.display = 'flex';
            errorAlert.classList.add('fade-in');
        }
    },
    
    hideError() {
        const errorAlert = document.getElementById('errorAlert');
        if (errorAlert) {
            errorAlert.style.display = 'none';
        }
    }
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
    
    test() {
        const pageSizeSelector = document.getElementById('pageSizeSelector');
        if (pageSizeSelector) {
            console.log('Page size selector found, current value:', pageSizeSelector.value);
            alert('Page size selector is working! Current value: ' + pageSizeSelector.value);
        } else {
            console.log('Page size selector not found');
            alert('Page size selector not found!');
        }
    }
};

// Form and button event handlers
const EventHandlers = {
    initializeSearchForm() {
        const searchForm = document.getElementById('searchForm');
        if (searchForm) {
            searchForm.addEventListener('submit', function(e) {
                LoadingManager.showButtonLoading('searchButton');
                LoadingManager.hideError();
                
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
                LoadingManager.hideError();
                
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
                LoadingManager.hideError();
                
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
    
    initializeAjaxSearchButton() {
        const ajaxSearchButton = document.getElementById('ajaxSearchButton');
        if (ajaxSearchButton) {
            ajaxSearchButton.addEventListener('click', function() {
                const clientCode = document.querySelector('input[asp-for="ClientCode"]').value;
                const authorityKey = document.querySelector('input[asp-for="AuthorityKey"]').value;
                
                LoadingManager.showButtonLoading('ajaxSearchButton');
                LoadingManager.showProgressBar();
                LoadingManager.showTableLoading();
                LoadingManager.hideError();
                
                // Simulate progress updates
                let progress = 0;
                const progressInterval = setInterval(() => {
                    progress += 10;
                    LoadingManager.updateProgress(progress);
                    if (progress >= 100) {
                        clearInterval(progressInterval);
                    }
                }, 100);
                
                // Make AJAX request
                fetch(`/api/UserProfileApi?clientCode=${encodeURIComponent(clientCode)}&authorityKey=${encodeURIComponent(authorityKey)}`)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Failed to fetch data');
                        }
                        return response.json();
                    })
                    .then(data => {
                        LoadingManager.hideProgressBar();
                        LoadingManager.hideTableLoading();
                        LoadingManager.hideButtonLoading('ajaxSearchButton');
                        
                        if (data && data.length > 0) {
                            TableManager.updateTable(data);
                        } else {
                            LoadingManager.showEmptyState();
                        }
                    })
                    .catch(error => {
                        LoadingManager.hideProgressBar();
                        LoadingManager.hideTableLoading();
                        LoadingManager.hideButtonLoading('ajaxSearchButton');
                        // Error handling can be customized per page
                        console.error('AJAX Error:', error);
                    });
            });
        }
    }
};

// Table management utilities
const TableManager = {
    updateTable(data) {
        const tbody = document.getElementById('resultsTableBody');
        if (!tbody) return;
        
        tbody.innerHTML = '';
        
        data.forEach(row => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${row.clientCode || ''}</td>
                <td>${row.authorityKey || ''}</td>
                <td>${row.currency || ''}</td>
                <td>${row.active || ''}</td>
                <td>${row.taxType || ''}</td>
                <td>${row.taxBase || ''}</td>
                <td>${row.lastMaintained ? new Date(row.lastMaintained).toLocaleDateString('en-GB') : ''}</td>
                <td>${row.createdOn ? new Date(row.createdOn).toLocaleDateString('en-GB') : ''}</td>
                <td>${row.updatedOn ? new Date(row.updatedOn).toLocaleDateString('en-GB') : 'N/A'}</td>
            `;
            tbody.appendChild(tr);
        });
        
        const tableContainer = document.getElementById('tableContainer');
        if (tableContainer) {
            tableContainer.classList.add('fade-in');
        }
    }
};

// Main initialization function
function initializePage() {
    // Hide any initial loading states
    LoadingManager.hidePageLoading();
    LoadingManager.hideError();
    
    // Clear any persisted error states
    const errorAlert = document.getElementById('errorAlert');
    if (errorAlert) {
        errorAlert.style.display = 'none';
    }
    
    // Initialize all components
    PageSizeManager.initialize();
    EventHandlers.initializeSearchForm();
    EventHandlers.initializeRefreshButton();
    EventHandlers.initializeExportButton();
    EventHandlers.initializeBackButton();
    
    // AJAX search is disabled by default, but can be enabled per page
    // EventHandlers.initializeAjaxSearchButton();
    
    console.log('Page utilities initialized successfully');
}

// Export functions for use in other pages
window.PaginationUtils = {
    LoadingManager,
    PageSizeManager,
    EventHandlers,
    TableManager,
    initializePage
};

// Auto-initialize when DOM is ready
document.addEventListener('DOMContentLoaded', initializePage); 