
/**
 * AutoDelete Ajax Action Module
 * =============================
 * Lightweight utility for triggering Ajax actions (delete, approve, etc.)
 * directly from buttons or links with SweetAlert2 UI feedback.
 *
 * This module is commonly used with SmartDataTable row actions but
 * works with any element on the page.
 *
 * The system relies entirely on HTML data attributes.
 *
 *
 * ------------------------------------------------------------
 * Dependencies
 * ------------------------------------------------------------
 *
 * - jQuery
 * - SweetAlert2
 * - SmartDataTable (optional, only needed for table reload)
 *
 *
 * ------------------------------------------------------------
 * Quick Start
 * ------------------------------------------------------------
 *
 * 1) Include this script after jQuery and SweetAlert2.
 *
 * 2) Add the attribute `data-ad-url` to any element.
 *
 * Example:
 *
 * <button
 *   data-ad-url="/api/users/delete"
 *   data-ad-param-id="12">
 *   Delete
 * </button>
 *
 * When clicked:
 *
 * - A confirmation dialog appears
 * - Ajax request is sent
 * - SweetAlert success/error message is shown
 * - Optional SmartDataTable reload happens
 *
 *
 * ------------------------------------------------------------
 * Required Attribute
 * ------------------------------------------------------------
 *
 * data-ad-url="API_URL"
 *
 * The endpoint that will receive the Ajax request.
 *
 *
 * ------------------------------------------------------------
 * Optional Attributes
 * ------------------------------------------------------------
 *
 * data-ad-method="POST|GET|PUT|DELETE"
 *   HTTP request method.
 *   Default: POST
 *
 * data-ad-table="tableId"
 *   SmartDataTable ID to reload after success.
 *
 * data-ad-close-after="milliseconds"
 *   Time before success message auto closes.
 *   Default: 2500
 *
 *
 * ------------------------------------------------------------
 * Sending Data to Server
 * ------------------------------------------------------------
 *
 * Two supported methods exist.
 *
 *
 * 1) JSON Payload
 *
 * data-ad-payload='{"id":12,"force":true}'
 *
 * Request body:
 *
 * {
 *   "id": 12,
 *   "force": true
 * }
 *
 *
 * 2) Parameter Attributes (Recommended)
 *
 * Any attribute starting with:
 *
 * data-ad-param-*
 *
 * will automatically be converted into request parameters.
 *
 * Example:
 *
 * data-ad-param-id="12"
 * data-ad-param-status="active"
 *
 * Resulting request body:
 *
 * {
 *   "id": 12,
 *   "status": "active"
 * }
 *
 *
 * ------------------------------------------------------------
 * Language Configuration
 * ------------------------------------------------------------
 *
 * Default language: Persian (fa)
 *
 * You can change the language globally:
 *
 * window.AutoDeleteDefaults = {
 *   lang: 'en'
 * }
 *
 * Supported languages:
 *
 * fa → Persian
 * en → English
 *
 *
 * ------------------------------------------------------------
 * Example: SmartDataTable Delete Button
 * ------------------------------------------------------------
 *
 * <button class="btn btn-danger btn-sm"
 *   data-ad-url="/api/admin/users/delete"
 *   data-ad-table="usersTable"
 *   data-ad-param-id="{{row.id}}">
 *   <i class="fas fa-trash"></i>
 * </button>
 *
 *
 * ------------------------------------------------------------
 * Request Flow
 * ------------------------------------------------------------
 *
 * 1) User clicks button
 * 2) Confirmation dialog appears
 * 3) Loading dialog is shown
 * 4) Ajax request is sent
 * 5) Success or error message appears
 * 6) Table reloads if data-ad-table is defined
 *
 *
 * ------------------------------------------------------------
 * Notes
 * ------------------------------------------------------------
 *
 * - Works with dynamically generated elements (event delegation).
 * - Designed to stay lightweight and framework‑agnostic.
 * - Ideal for CRUD actions in admin panels and data tables.
 *
 */
