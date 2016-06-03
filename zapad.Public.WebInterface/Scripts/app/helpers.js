var App = App || {};

/** TODO - refactor */
App.Helpers = {
    basePath: "",
    setBasePath: function (basePath) {
        App.Helpers._basePath = basePath;
    },
    getBasePath: function () {
        return App.Helpers._basePath;
    },
    getAjaxUrl: function (url) {
        var ajaxUrl = (url.charAt(0) !== "/") ? "/" + url : url;

        return App.Helpers.getBasePath() + ajaxUrl;
    },

    getDataList: function (data, className) {
        var result = "",
            className = className || ""
        ;

        if (data && data.length) {
            result += "<ul class=\"" + className + "\">";
            $.each(data, function (index, elem) {
                result += "<li>" + elem + "</li>";
            });
            result += "</ul>";
        }

        return (result) ? result : "-";
    },

    getGridFilterDataSource: function (data, field) {
        var result = [],
            uniqFilters = {}
        ;

        /** Get only uniq filters */
        $.each(data, function (index, element) {
            var searchKey = (element[field]) ? field : "Value";

            if (element[searchKey]) {
                uniqFilters[element[searchKey]] = "";
            }
        });

        /** Format data */
        $.each(uniqFilters, function (key, val) {
            var resultItem = {};
            resultItem[field] = key;
            result.push(resultItem);
        });

        return result;
    },

    getGridFilterDataSourceExt: function (data, key, field) {
        var result = [];

        $.each(data, function (index, element) {
            App.Helpers.renameObjectKey(element, field, key);
        });

        return App.Helpers.getGridFilterDataSource(data, field);
    },

    renameObjectKey: function(obj, newName, oldName){
        if (oldName == newName) {
            return obj;
        }
        if (obj.hasOwnProperty(oldName)) {
            obj[newName] = obj[oldName];
            delete obj[oldName];
        }
        return obj;
    },

    getObjectCountKeys: function (obj) {
        var count = 0;
        for (var i in obj) {
            if (obj.hasOwnProperty(i)) {
                count++;
            }
        }

        return count;
    },

    error403: function () {
        alert("Ошибка 403. Нет доступа.");
    },

    reload: function () {
        window.location.reload();
    },

    redirect: function (url) {
        window.location.href = url;
    },

    getFormatFormData: function (formData, withMultiplyValue) {
        var result = {},
            withMultiplyValue = (withMultiplyValue === true) ? true : false
        ;

        if (App.Helpers.isObjectIterable(formData)) {
            for (i = 0; i < formData.length; i++) {
                var a = formData[i],
                    name = $.trim(a.name),
                    value = $.trim(a.value);

                if (!withMultiplyValue) {
                    //Default
                    result[name] = value;
                } else {
                    //Multiply
                    if (name in result) {
                        var oldValue = result[name];

                        if ($.isArray(oldValue)) {
                            oldValue.push(value);
                            var newValue = oldValue;
                        } else {
                            var newValue = [oldValue, value];
                        }
                        result[name] = newValue;
                    } else {
                        result[name] = value;
                    }
                }
            }
        }

        return result;
    },

    /** Get promise ajax */
    getPromiseAjax: function ($parent, $context) {
        var ajaxRequiredFields = $context.attr("data-ajax-required-fields").split(','),
            ajaxReceiver = $context.attr("data-ajax-receiver"),
            ajaxParams = {},
            ajaxCT = $parent.attr("data-ajax-ct")
        ;

        /** Set params */
        $.each(ajaxRequiredFields, function (index, element) {
            ajaxParams[element] = $parent.find("[name=" + element + "]").val();
        });

        /// <summary>
        /// Получение различных списков свзязанных данных
        /// </summary>  
        /// <param name="receiver" type="string">Получатель</param>
        /// <param name="args" type="string">Параметры</param>
        /// <param name="CT" type="int">Contact type</param>    
        /// <returns>
        ///     <request name="rc" type="int">Код состояния ответа - если rc == 0, то ответ корректный, если нет, то произошла какая то ошибка</request>
        ///     <request name="List" type="List<PropertyElementAjax>">Список данных</request>
        ///     <request name="ListAdvanced" type="List<PropertyElementAjax>">Дополнительный список данных</request>
        /// </returns>
        var promise = $.ajax({
            type: "POST",
            url: "/Main/GetPropertiesAjax",
            data: {
                'args': JSON.stringify(ajaxParams),
                'receiver': ajaxReceiver,
                'CT': ajaxCT
            },
        });

        return promise;
    },

    isObjectIterable: function (obj) {
        return (obj && ($.isPlainObject(obj) || $.isArray(obj))) ? true : false;
    },

    resetContainer: function ($container) {
        if ($container && $container.length) {
            $container.find(':input').not("[data-cancel-clear=1]").each(function () {
                switch (this.type) {
                    case "password":
                    case "text":
                    case "textarea":
                    case "file":
                    case "select-one":
                    case "hidden":
                        $(this).val("");
                        break;
                    case "checkbox":
                    case "radio":
                        this.checked = false;
                }
            });
        }
    },

    clearForm: function ($form, params) {
        if ($form && $form.length) {
            var params = params || {},
                clearForm = (params.clearForm === false) ? false : true,
                clearContactFormOnly = (params.clearContactFormOnly === true) ? true : false
            ;

            /** Not clear form */
            if (!clearForm) {
                return;
            }

            /** Clear only contact */
            if (clearContactFormOnly) {
                $form.find("[name=peopleid]").val("");
                $form.find("[name=phone]").val("");
                $form.find("[name=lastname]").val("");
                $form.find("[name=firstname]").val("");
                $form.find("[name=middlename]").val("");

                return;
            }
            /** TODO - fix */

            /** City/district */
            var $formCity = $form.find("[name=city]");
            if ($formCity.length) {
                /** Remove selected */
                $formCity.find("option:selected").prop("selected", false);

                /** Find first element */
                $formCityFirst = $formCity.find("option:first");

                /** Select first element */
                $formCityFirst.prop("selected", true);

                /** Trigger event */
                $formCityFirst.trigger("change");
            }

            /** Commercial */
            var $formCommercial = $form.find("[name=comercial]");
            if ($formCommercial.length && $formCommercial.prop("checked")) {
                $formCommercial.prop("checked", false);
                $formCommercial.trigger("change");
            }

            /** Fix bug with 0-0 budget */
            App.Helpers.resetContainer($form);
            /** Reset all form elements */
            //$form[0].reset();
            /** Reset hidden elements */
            //$form.find("[type=hidden]").not("[data-cancel-clear=1]").val("");
            /** Fix bug with 0-0 budget */

            /** 
             * Reset customrangeslider 
             * Important - reset after change commercial and reset form
             */
            $form.find("[data-role=customrangeslider]").each(function (index, customRangeSlider) {
                var $customRangeSlider = $(customRangeSlider).data("kendoCustomRangeSlider");
                $customRangeSlider.reset();
            });
            $form.find("[data-role=numerictextbox]").each(function (index, elem) {
                var $elem = $(elem).data("kendoNumericTextBox"),
                    defaultValue = $elem.element.attr("value")
                ;

                if (defaultValue) {
                    $elem.value(defaultValue);
                }
            });


            /** Clear filter state */
            $form.find(".child-item-proxy-data").html("");
            $form.find(".child-item-status").html("").addClass("hide");

            return;
        }
    },

    _formData: {},
    getFormData: function () {
        return App.Helpers._formData;
    },
    setFormData: function (formData) {
        App.Helpers._formData = formData;
    },

    _formAutocompleteData: {},
    getFormAutocompleteData: function () {
        return App.Helpers._formAutocompleteData;
    },
    setFormAutocompleteData: function (formAutocompleteData) {
        App.Helpers._formAutocompleteData = formAutocompleteData;
    },



    fillForm: function ($form, options) {
        var data = App.Helpers.getFormData();
        if ($form && $form.length && $.isPlainObject(data) && !$.isEmptyObject(data)) {
            $.each(data, function (name, value) {
                /** Get form element */
                var $formElement = $form.find("[name=" + name + "]");

                /** Get non default form element (kendo and etc) */
                if (!$formElement.length) {
                    $formElement = $form.find("[data-property-name=" + name + "]");
                }

                if ($formElement.length) {
                    switch (true) {
                        case $formElement.is("input[type=text]"):
                        case $formElement.is("input[type=hidden]"):
                        case $formElement.is("textarea"):
                            $formElement.val(value[0]);
                            break;
                        case $formElement.is("select"):
                            $formElement.find("option[value=" + value[0] + "]").each(function (index, el) {
                                var $el = $(el);
                                $el.prop("selected", true);
                                $el.trigger("change", [{ autoTriggeredEvent: true }]);
                            });

                            if ($formElement.attr("name") === "district") {
                                setTimeout(function () {
                                    $formElement.find("option[value=" + value[0] + "]").each(function (index, el) {
                                        var $el = $(el);
                                        $el.prop("selected", true);
                                        $el.trigger("change", [{ autoTriggeredEvent: true }]);
                                    });
                                }, 500);
                            }

                            if ($formElement.attr("name") === "location_district") {
                                setTimeout(function () {
                                    $formElement.find("option[value=" + value[0] + "]").each(function (index, el) {
                                        var $el = $(el);
                                        $el.prop("selected", true);
                                        $el.trigger("change", [{ autoTriggeredEvent: true }]);
                                    });
                                }, 500);
                            }

                            break;
                        case $formElement.is(":radio"):
                        case $formElement.is(":checkbox"):
                            $.each(value, function (valueIndex, valueItem) {
                                $formElement.filter("[value=" + valueItem + "]").each(function (index, el) {
                                    var $el = $(el);
                                    $el.prop("checked", true);
                                    $el.trigger("change", [{ silentMode: true, autoTriggeredEvent: true, fillChilData: true }]);
                                });
                            });

                            break;
                        case $formElement.is('[data-role="customrangeslider"]'):
                            $formElement.data("kendoCustomRangeSlider").updateValue(value, true);
                            break;
                    }
                }
            });
        }
    },

    fillFormAutocomplete: function ($form, $kendoFormContactList, options) {
        var data = App.Helpers.getFormAutocompleteData();
        var
            $formClientHistory = $form.find(".b-client-history"),
            $formClientFlatStateHistory = $form.find(".b-client-flat-state-history"),
            $formPropertPhone = $form.find("[name=phone]"),
            $formPropertPhoneState = $form.find(".phone-state"),
            $formPropertFirstname = $form.find("[name=firstname]"),
            $formPropertMiddlename = $form.find("[name=middlename]"),
            $formPropertLastname = $form.find("[name=lastname]"),
            $formPropertGender = $form.find("[name=gender]"),
            $formPropertPeopleId = $form.find("[name=peopleid]"),
            $formPropertBirthday = $form.find("[name=birthday]"),
            $formPropertCids = $form.find("[name=cids]"),
            $formPropertComment = $form.find("[name=client_comment]")
        ;

        if ($kendoFormContactList) {
            /** Clear contact list */
            $kendoFormContactList.dataSource.data([]);

            /** Set contact list */
            var DataItemCids = [];
            if (data.Advanced) {
                $.each(data.Advanced, function (index, element) {
                    /** Set cids */
                    DataItemCids.push(element.ContactId);

                    /** Set data source element */
                    var dataSourceElement = {
                        ItemId: element.ContactId,
                        ContactTypeId: element.ContactTypeId,
                        ItemValue: element.ContactTitle,
                        ItemDesc: element.ContactDescription
                    };
                    $kendoFormContactList.dataSource.add(dataSourceElement);
                });
            }

            $kendoFormContactList.dataSource.fetch(function () {
                if (!$kendoFormContactList.dataSource.total()) {
                    $kendoFormContactList.addRow();
                }
            });

            $formPropertCids.val(DataItemCids.join(","));
        }

        /** Set FIO && id */
        $formPropertPeopleId.val(data.PeopleId);
        $formPropertPhone.val(data.Phone);
        $formPropertFirstname.val(data.I);
        $formPropertMiddlename.val(data.O);
        $formPropertLastname.val(data.F);

        /** Set gender*/
        $formPropertGender.each(function (index, element) {
            var $element = $(element)
            currenValue = ($element.val() == 1) ? 1 : 0,
            newValue = (data.Gender !== "0") ? 1 : 0
            ;

            /** Reset all */
            $element.prop("checked", false);

            /** Set new */
            if (newValue == currenValue) {
                $element.prop("checked", true);
            }
        });

        /** Set birthday */
        $formPropertBirthday.val(data.BDate);

        /** Set state button */
        $formPropertPhoneState.removeClass("hide");

        /** Set client comment */
        $formPropertComment.val(data.Comment);

        /** Set history */
        if (data.History) {
            var historyMessage = "";
            $.each(data.History, function (index, item) {
                historyMessage += "<p><strong>" + item.Contacted + "</strong>: " + item.TypeContact + " - менеджер " + item.Employee + "</p>";
            });
            $formClientHistory.find(".b-content").html(historyMessage);
            (historyMessage) ? $formClientHistory.removeClass("hide") : $formClientHistory.addClass("hide");
        }

        /** Set flat state history */

        var flatStateHistoryMessage = $.trim(data.FlatStateHistory) ? $.trim(data.FlatStateHistory) : "";
        $formClientFlatStateHistory.find(".b-content").html(flatStateHistoryMessage);
        (flatStateHistoryMessage) ? $formClientFlatStateHistory.removeClass("hide") : $formClientFlatStateHistory.addClass("hide");
    },

    clearFormAutocomplete: function ($form, $kendoFormContactList) {
        var $formClientHistory = $form.find(".b-client-history"),
            $formClientFlatStateHistory = $form.find(".b-client-flat-state-history"),
            $formPropertPhoneState = $form.find(".phone-state")
        ;

        if ($kendoFormContactList) {
            /** Clear grid */
            $kendoFormContactList.dataSource.data([]);
            $kendoFormContactList.addRow();
        }

        /** Clear history */
        $formClientHistory.find(".b-content").html("");
        $formClientHistory.addClass("hide");

        /** Clear flat state history */
        $formClientFlatStateHistory.find(".b-content").html("");
        $formClientFlatStateHistory.addClass("hide");

        /** Set state */
        $formPropertPhoneState.addClass("hide");
    },

    InitStartFilterValue: function(grid, dropdownlist, keyField)
    {
        grid.thead.find("th:last")
                    .data("kendoFilterMenu")
                        .filterModel.filters[0].value = dropdownlist.dataSource.data()[0][keyField];
    },

    createDropDownFilter: function(e, data, url, method, key, value, initStartValue, $blockMessage) {
        if (url != null) {
            e.container.find("div.k-filter-help-text").css("display", "none");
            e.container.find("span.k-dropdown:first").css("display", "none");
            var $dropdown = e.container.find(".k-textbox:first")
            .removeClass("k-textbox")
            .kendoDropDownList({
                dataSource: {
                    transport: {
                        read: function (e) {
                            $.ajax({
                                type: method,
                                url: url,
                                dataType: "json",
                                success: function (data) {
                                    if (data.rc == 0) {
                                        e.success(data.Items);
                                    } else if (data.rc == 403) {
                                        App.Helpers.error403();
                                        e.success([]);
                                    } else {
                                        App.Message.showErrorMessage("Во время получения данных произошла ошибка с кодом " + data.rc + ".", $blockMessage);
                                        e.success([]);
                                    }
                                },
                                error: function (data) {
                                    App.Message.showErrorMessage("Во время загрузки данных произошла неизвестная ошибка.", $blockMessage);
                                    e.success([]);
                                }
                            });
                        }
                    }
                },
                dataTextField: value,
                dataValueField: key
            }).data("kendoDropDownList")
        }
        else if (data != null) {
            e.container.find("div.k-filter-help-text").css("display", "none");
            e.container.find("span.k-dropdown:first").css("display", "none");
            var $dropdown = e.container.find(".k-textbox:first")
            .removeClass("k-textbox")
            .kendoDropDownList({
                dataSource: new kendo.data.DataSource({
                    data: data
                }),
                dataTextField: value,
                dataValueField: key
            }).data("kendoDropDownList")

            if (initStartValue) {
                App.Helpers.InitStartFilterValue($grid, $dropdown, key)
            }
        }
    },

    removeFiltersForField: function(expression, field) {
        if (expression.filters) {
            expression.filters = $.grep(expression.filters, function (filter) {
                App.Helpers.removeFiltersForField(filter, field);
                if (filter.filters) {
                    return filter.filters.length;
                } else {
                    return filter.field != field;
                }
            });
        }
    },

    createMultiSelectFilter: function (e, data, url, method, key, value, $blockMessage) {
        if (url && method) {
            //убираем заголовок контейнера
            e.container.find("div.k-filter-help-text").css("display", "none");
            var popup = e.container.data("kendoPopup");
            //datasource грида, поэтому вызов через call
            var dataSource = this.dataSource;
            var field = e.field;
            //datasource для фильтра
            var checkboxesDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (e) {
                        $.ajax({
                            type: method,
                            url: url,
                            dataType: "json",
                            success: function (data) {
                                if (data.rc == 0) {
                                    e.success(data.Items);
                                } else if (data.rc == 403) {
                                    App.Helpers.error403();
                                    e.success([]);
                                } else {
                                    App.Message.showErrorMessage("Во время получения данных произошла ошибка с кодом " + data.rc + ".", $blockMessage);
                                    e.success([]);
                                }
                            },
                            error: function (data) {
                                App.Message.showErrorMessage("Во время загрузки данных произошла неизвестная ошибка.", $blockMessage);
                                e.success([]);
                            }
                        });
                    }
                }
            });

            //находим скрытый заголов контейнера
            var helpTextElement = e.container.children(":first").children(":first");
            helpTextElement.nextUntil(":has(.k-button)").remove();

            //фильтр
            var element = $("<div class='checkbox-ontainer'></div>").insertAfter(helpTextElement).kendoListView({
                dataSource: checkboxesDataSource,
                template: "<div><input type='checkbox' value='#:" + key + "#'/>#:" + value + "#</div>"
            });

            //логика фильтра
            e.container.find("[type='submit']").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                var filter = dataSource.filter() || { logic: "and", filters: [] };
                var fieldFilters = $.map(element.find(":checkbox:checked"), function (input) {
                    return {
                        field: field,
                        operator: "eq",
                        value: input.value
                    };
                });
                if (fieldFilters.length) {
                    App.Helpers.removeFiltersForField(filter, field);
                    filter.filters.push({
                        logic: "or",
                        filters: fieldFilters
                    });
                    dataSource.filter(filter);
                }
                popup.close();
            });
        }
        else if (data != null) {
            e.container.find("div.k-filter-help-text").css("display", "none");
            var popup = e.container.data("kendoPopup");
            var dataSource = this.dataSource;
            var field = e.field;
            var checkboxesDataSource = new kendo.data.DataSource({
                data: data
            });

            var helpTextElement = e.container.children(":first").children(":first");
            helpTextElement.nextUntil(":has(.k-button)").remove();
            var element = $("<div class='checkbox-ontainer'></div>").insertAfter(helpTextElement).kendoListView({
                dataSource: checkboxesDataSource,
                template: "<div><input type='checkbox' value='#:" + key + "#'/>#:" + value + "#</div>"
            });
            e.container.find("[type='submit']").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                var filter = dataSource.filter() || { logic: "and", filters: [] };
                var fieldFilters = $.map(element.find(":checkbox:checked"), function (input) {
                    return {
                        field: field,
                        operator: "eq",
                        value: input.value
                    };
                });
                if (fieldFilters.length) {
                    App.Helpers.removeFiltersForField(filter, field);
                    filter.filters.push({
                        logic: "or",
                        filters: fieldFilters
                    });
                    dataSource.filter(filter);
                }
                popup.close();
            });
        }
    }

};