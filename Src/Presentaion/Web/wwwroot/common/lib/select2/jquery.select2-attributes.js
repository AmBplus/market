/**
 * Select2 Attribute Plugin
 * Zero-JS configuration for Select2 via HTML data attributes
 * Version: 1.0.0
 */
(function ($) {
  "use strict";

  // ─── Helpers ────────────────────────────────────────────────────────────────

  function getNestedValue(obj, path) {
    if (!path) return obj;
    return path.split(".").reduce(function (acc, key) {
      return acc && acc[key] !== undefined ? acc[key] : undefined;
    }, obj);
  }

  function resolveCallback(name) {
    if (!name) return null;
    var fn = name.split(".").reduce(function (acc, key) {
      return acc && acc[key] !== undefined ? acc[key] : undefined;
    }, window);
    return typeof fn === "function" ? fn : null;
  }

  function parseAttr($el, attr, fallback) {
    var val = $el.data(attr);
    return val !== undefined && val !== "" ? val : fallback;
  }

  function parseBool($el, attr, fallback) {
    var val = $el.data(attr);
    if (val === undefined || val === "") return fallback;
    if (val === true || val === "true" || val === 1) return true;
    if (val === false || val === "false" || val === 0) return false;
    return fallback;
  }

  function getDropdownParent($el) {
    var custom = $el.data("dropdown-parent");
    if (custom) return $(custom);
    var $modal = $el.closest(".modal");
    if ($modal.length) return $modal;
    var $container = $el.closest("[data-select2-container]");
    if ($container.length) return $container;
    return $(document.body);
  }

  // ─── Core Init ──────────────────────────────────────────────────────────────

  function initSelect2Attr($el) {
    if ($el.data("s2attr-initialized")) return;
    $el.data("s2attr-initialized", true);

    var url         = parseAttr($el, "url", null);
    var dataPath    = parseAttr($el, "data-path", "data");
    var idKey       = parseAttr($el, "id-key", "id");
    var textKey     = parseAttr($el, "text-key", "text");
    var pagination  = parseBool($el, "pagination", false);
    var pageSize    = parseInt(parseAttr($el, "page-size", 10));
    var delay       = parseInt(parseAttr($el, "delay", 300));
    var searchParam = parseAttr($el, "search-param", "search");
    var pageParam   = parseAttr($el, "page-param", "page");
    var pageSizeParam = parseAttr($el, "page-size-param", "pageSize");
    var totalPath   = parseAttr($el, "total-path", null);
    var hasMorePath = parseAttr($el, "has-more-path", null);
    var currentId   = parseAttr($el, "current-id", null);
    var dependsOn   = parseAttr($el, "depends-on", null);
    var depParam    = parseAttr($el, "dep-param", "parentId");
    var placeholder = parseAttr($el, "placeholder", $el.data("placeholder") || "انتخاب کنید...");
    var noResults   = parseAttr($el, "no-results", "نتیجه‌ای یافت نشد");
    var allowClear  = parseBool($el, "allow-clear", true);
    var minimumInputLength = parseInt(parseAttr($el, "minimum-input-length", 0));
    var maximumSelectionLength = parseInt(parseAttr($el, "maximum-selection-length", 0));
    var multiple    = $el.prop("multiple") || parseBool($el, "multiple", false);
    var tags        = parseBool($el, "tags", false);
    var theme       = parseAttr($el, "theme", "bootstrap-5");
    var dir         = parseAttr($el, "dir", $("html").attr("dir") || "ltr");
    var language    = parseAttr($el, "language", "en");

    // callbacks
    var onSelect    = resolveCallback(parseAttr($el, "on-select", null));
    var onClear     = resolveCallback(parseAttr($el, "on-clear", null));
    var onLoad      = resolveCallback(parseAttr($el, "on-load", null));
    var onChange    = parseAttr($el, "on-change", null); // selector(s) to update
    var watch       = parseAttr($el, "watch", null);     // selector(s) to watch

    // ─── Build Select2 Options ────────────────────────────────────────────────

    var s2Options = {
      theme: theme,
      dir: dir,
      placeholder: placeholder,
      allowClear: allowClear,
      dropdownParent: getDropdownParent($el),
      language: {
        noResults: function () { return noResults; },
        searching: function () { return "در حال جستجو..."; },
        loadingMore: function () { return "در حال بارگذاری..."; },
        inputTooShort: function (args) {
          return "حداقل " + args.minimum + " کاراکتر وارد کنید";
        }
      }
    };

    if (minimumInputLength > 0) s2Options.minimumInputLength = minimumInputLength;
    if (maximumSelectionLength > 0) s2Options.maximumSelectionLength = maximumSelectionLength;
    if (multiple) s2Options.multiple = true;
    if (tags) s2Options.tags = true;

    // ─── AJAX Mode ────────────────────────────────────────────────────────────

    if (url) {
      s2Options.ajax = {
        url: url,
        dataType: "json",
        delay: delay,
        data: function (params) {
          var query = {};
          query[searchParam] = params.term || "";

          if (pagination) {
            query[pageParam] = params.page || 1;
            query[pageSizeParam] = pageSize;
          }

          // dependency injection
          if (dependsOn) {
            var depSelectors = dependsOn.split(",").map(function (s) { return s.trim(); });
            depSelectors.forEach(function (sel) {
              var $dep = $(sel);
              if ($dep.length) {
                var depName = $dep.data("dep-param") || depParam;
                var depVal  = $dep.val();
                if (depVal) query[depName] = depVal;
              }
            });
          }

          return query;
        },
        processResults: function (response, params) {
          var items = getNestedValue(response, dataPath) || [];

          var results = items.map(function (item) {
            return {
              id:   getNestedValue(item, idKey),
              text: getNestedValue(item, textKey),
              _raw: item
            };
          });

          if (typeof onLoad === "function") {
            onLoad(response, results, $el[0]);
          }

          var paginationResult = { more: false };

          if (pagination) {
            if (hasMorePath) {
              paginationResult.more = !!getNestedValue(response, hasMorePath);
            } else if (totalPath) {
              var total = getNestedValue(response, totalPath) || 0;
              var page  = params.page || 1;
              paginationResult.more = page * pageSize < total;
            } else {
              paginationResult.more = results.length >= pageSize;
            }
          }

          return { results: results, pagination: paginationResult };
        },
        cache: true
      };
    }

    // ─── Init Select2 ─────────────────────────────────────────────────────────

    $el.select2(s2Options);

    // ─── Load Initial Value ───────────────────────────────────────────────────

    if (currentId && url) {
      fetchAndSetInitialValue($el, currentId, url, idKey, textKey, dataPath, searchParam);
    }

    // ─── Event Bindings ───────────────────────────────────────────────────────

    $el.on("select2:select", function (e) {
      var data = e.params.data;
      if (typeof onSelect === "function") {
        onSelect(data, $el[0]);
      }
      triggerOnChange($el, onChange);
    });

    $el.on("select2:unselect select2:clear", function (e) {
      if (typeof onClear === "function") {
        onClear($el[0]);
      }
      triggerOnChange($el, onChange);
    });

    $el.on("change", function () {
      triggerOnChange($el, onChange);
    });

    // ─── Dependency Handling ──────────────────────────────────────────────────

    if (dependsOn) {
      var depSelectors = dependsOn.split(",").map(function (s) { return s.trim(); });
      depSelectors.forEach(function (sel) {
        $(document).on("change", sel, function () {
          var val = $(this).val();
          $el.val(null).trigger("change");
          if ($el.data("select2")) {
            $el.select2("destroy");
            $el.data("s2attr-initialized", false);
            initSelect2Attr($el);
          }
          if (!val || val === "" || (Array.isArray(val) && val.length === 0)) {
            $el.prop("disabled", true).trigger("change.select2");
          } else {
            $el.prop("disabled", false);
          }
        });
      });

      // disable initially if parent has no value
      var hasParentValue = depSelectors.some(function (sel) {
        var v = $(sel).val();
        return v && v !== "";
      });
      if (!hasParentValue) {
        $el.prop("disabled", true);
      }
    }

    // ─── Watch Handling ───────────────────────────────────────────────────────

    if (watch) {
      var watchSelectors = watch.split(",").map(function (s) { return s.trim(); });
      watchSelectors.forEach(function (sel) {
        $(document).on("change", sel, function () {
          var action = parseAttr($el, "watch-action", "refresh"); // refresh | clear
          if (action === "clear") {
            $el.val(null).trigger("change");
          } else {
            // refresh: re-open or just clear cache
            if ($el.data("select2")) {
              $el.val(null).trigger("change");
              if ($el.data("s2attr-ajax-cache")) {
                $el.data("s2attr-ajax-cache", null);
              }
            }
          }
        });
      });
    }
  }

  // ─── Fetch Initial Value ─────────────────────────────────────────────────────

  function fetchAndSetInitialValue($el, id, url, idKey, textKey, dataPath, searchParam) {
    var fetchUrl = parseAttr($el, "fetch-url", url);
    var idParam  = parseAttr($el, "fetch-id-param", "id");

    var params = {};
    params[idParam] = id;

    $.ajax({
      url: fetchUrl,
      data: params,
      dataType: "json",
      success: function (response) {
        var items = getNestedValue(response, dataPath) || [];
        var item  = null;

        if (Array.isArray(items) && items.length > 0) {
          item = items[0];
        } else if (typeof items === "object" && !Array.isArray(items)) {
          item = items;
        }

        if (item) {
          var option = new Option(
            getNestedValue(item, textKey),
            getNestedValue(item, idKey),
            true,
            true
          );
          $el.append(option).trigger("change");
        }
      }
    });
  }

  // ─── Trigger onChange ────────────────────────────────────────────────────────

  function triggerOnChange($el, onChange) {
    if (!onChange) return;
    var targets = onChange.split(",").map(function (s) { return s.trim(); });
    targets.forEach(function (sel) {
      var $target = $(sel);
      if ($target.length && $target.data("s2attr-initialized")) {
        $target.val(null).trigger("change");
        $target.select2("destroy");
        $target.data("s2attr-initialized", false);
        initSelect2Attr($target);
      } else if ($target.length) {
        $target.trigger("change");
      }
    });
  }

  // ─── Auto-Init ───────────────────────────────────────────────────────────────

  function autoInit(context) {
    $("[data-select2-attr]", context || document).each(function () {
      initSelect2Attr($(this));
    });
  }

  // ─── MutationObserver for dynamic elements ────────────────────────────────────

  if (window.MutationObserver) {
    var observer = new MutationObserver(function (mutations) {
      mutations.forEach(function (mutation) {
        mutation.addedNodes.forEach(function (node) {
          if (node.nodeType !== 1) return;
          var $node = $(node);
          if ($node.is("[data-select2-attr]")) {
            initSelect2Attr($node);
          }
          $node.find("[data-select2-attr]").each(function () {
            initSelect2Attr($(this));
          });
        });
      });
    });
    observer.observe(document.body, { childList: true, subtree: true });
  }

  // ─── jQuery Plugin API ────────────────────────────────────────────────────────

  $.fn.select2Attr = function (options) {
    return this.each(function () {
      var $el = $(this);
      if (options === "destroy") {
        if ($el.data("select2")) $el.select2("destroy");
        $el.data("s2attr-initialized", false);
        return;
      }
      if (options === "refresh") {
        $el.val(null).trigger("change");
        if ($el.data("select2")) $el.select2("destroy");
        $el.data("s2attr-initialized", false);
      }
      initSelect2Attr($el);
    });
  };

  // ─── DOM Ready ────────────────────────────────────────────────────────────────

  $(function () {
    autoInit();
  });

})(jQuery);
