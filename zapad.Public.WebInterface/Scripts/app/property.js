var App = App || {};

/** TODO - refactor */
App.Property = {
    initContactAutocomplete: function ($form, $kendoFormContactList, params) {
        var params = params || {},
            showHistory = (params.showHistory) ? true : false,
            withFullClient = (params.withFullClient === true) ? true : false,
            callbackAfterLoadFullClient = (params.callbackAfterLoadFullClient && $.isFunction(params.callbackAfterLoadFullClient)) ? params.callbackAfterLoadFullClient : new Function()
        ;

        var _selectContactAutocomplete = function (DataItem) {
            /** Clear all form elements */
            App.Helpers.clearForm($form, params);

            /** Clear autocomplete */
            App.Helpers.clearFormAutocomplete($form, $kendoFormContactList);

            /** Set contacts */
            App.Helpers.setFormAutocompleteData(DataItem);
            App.Helpers.fillFormAutocomplete($form, $kendoFormContactList);

            /** Set all form elemnts */
            if (DataItem.PeopleId /**&& DataItem.History && DataItem.History.length*/) {
                /// <summary>
                /// Получение фильтров анкеты
                /// </summary>  
                /// <param name="PeopleId" type="long">Id клиента</param> 
                /// <returns>
                ///     <request name="rc" type="int">Код состояния ответа - если rc == 0, то ответ корректный, если нет, то произошла какая то ошибка</request>
                ///     <request name="filters" type="Dictionary<string, string[]>">Словарь с фильтрами</request>
                /// </returns>
                $.ajax({
                    type: "POST",
                    url: "/Main/GetAnketaFilters",
                    data: { PeopleId: DataItem.PeopleId },

                    beforeSend: function () {
                        kendo.ui.progress($form, true);
                    },
                    success: function (data) {
                        if (data.rc == 0) {
                            App.Helpers.setFormData(data.filters);
                            App.Helpers.fillForm($form);
                        } else if (data.rc == 403) {
                            App.Helpers.error403();
                        } else {
                            App.Message.showErrorMessage("Во время загрузки данных произошла ошибка с кодом " + data.rc + ".", $form);
                        }
                    },
                    error: function (data) {
                        App.Message.showErrorMessage("Во время загрузки данных произошла ошибка.", $form);
                    },
                    complete: function () {
                        kendo.ui.progress($form, false);
                    },
                });
            }

            if (withFullClient) {
                /// <summary>
                /// Получение полной контактной и паспортной информации о клиенте
                /// </summary>  
                /// <param name="PeopleId" type="long">Id клиента</param> 
                /// <returns>
                ///     <request name="rc" type="int">Код состояния ответа - если rc == 0, то ответ корректный, если нет, то произошла какая то ошибка</request>
                ///     <request name="model" type="ClientFull">Данные о клиенте</request>
                ///     <request name="modelHTML" type="string">Html клиента</request>
                /// </returns>
                $.ajax({
                    type: "POST",
                    url: "/Main/GetClientFull",
                    data: { PeopleId: DataItem.PeopleId },

                    beforeSend: function () {
                        kendo.ui.progress($form, true);
                    },
                    success: function (data) {
                        if (data.rc == 0) {
                            callbackAfterLoadFullClient(data);
                        } else if (data.rc == 403) {
                            App.Helpers.error403();
                        } else {
                            App.Message.showErrorMessage("Во время загрузки данных произошла ошибка с кодом " + data.rc + ".", $form);
                        }
                    },
                    error: function (data) {
                        App.Message.showErrorMessage("Во время загрузки данных произошла ошибка.", $form);
                    },
                    complete: function () {
                        kendo.ui.progress($form, false);
                    },
                });
            }
        };

        var _lastnameAutocomplete = function () {
            $form.find("[name=lastname]").kendoAutoComplete({
                dataSource: new kendo.data.DataSource({
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            /** Get current value */
                            var currentValue = $.trim(e.data.filter.filters[0].value);

                            /** Check length */
                            if (currentValue.length >= 3) {
                                /// <summary>
                                /// Поиск клиентов по фамилии и номеру телефона
                                /// </summary>  
                                /// <param name="phone" type="string">Телефон или его часть</param> 
                                /// <param name="surname" type="string">Фамилия или её част</param> 
                                /// <param name="showHistory" type="bool">Флаг показа истории</param> 
                                /// <returns>
                                ///     <request name="rc" type="int">Код состояния ответа - если rc == 0, то ответ корректный, если нет, то произошла какая то ошибка</request>
                                ///     <request name="list" type="List<FindedContact>">Список клиентов</request>
                                /// </returns>
                                $.ajax({
                                    type: "post",
                                    url: "/main/FormSearchByParamsAjax",
                                    data: { surname: currentValue, showHistory: showHistory },

                                    success: function (data) {
                                        if ((data.rc == 0) && (data.list.length)) {
                                            var newDataSource = data.list;
                                            e.success(data.list);
                                        } else if (data.rc == 403) {
                                            App.Helpers.error403();
                                            e.success([]);
                                        } else {
                                            e.success([]);
                                        }
                                    },
                                    error: function (data) {
                                        e.success([]);
                                    },
                                });
                            } else {
                                e.success([]);
                            }
                        }
                    },
                }),

                dataTextField: "F",
                dataValueld: "PeopleId",
                filter: "contains",
                template: "#:data.Phone#" + " (#:data.I# #:data.O# #:data.F#)",
                select: function (e) {
                    _selectContactAutocomplete(this.dataItem(e.item.index()));
                },
            });
        };

        var _phoneAutocomplete = function () {
            $form.find("[name=phone]").kendoAutoComplete({
                dataSource: new kendo.data.DataSource({
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            /** Get current value */
                            var currentValue = $.trim(e.data.filter.filters[0].value).replace("+", "");

                            /** Check length */
                            if (currentValue.length >= 3) {
                                /// <summary>
                                /// Поиск клиентов по фамилии и номеру телефона
                                /// </summary>  
                                /// <param name="phone" type="string">Телефон или его часть</param> 
                                /// <param name="surname" type="string">Фамилия или её част</param> 
                                /// <param name="showHistory" type="bool">Флаг показа истории</param> 
                                /// <returns>
                                ///     <request name="rc" type="int">Код состояния ответа - если rc == 0, то ответ корректный, если нет, то произошла какая то ошибка</request>
                                ///     <request name="list" type="List<FindedContact>">Список клиентов</request>
                                /// </returns>
                                $.ajax({
                                    type: "post",
                                    url: "/main/FormSearchByParamsAjax",
                                    data: { phone: currentValue, showHistory: showHistory },

                                    success: function (data) {
                                        if ((data.rc == 0) && (data.list.length)) {
                                            var newDataSource = data.list;
                                            e.success(data.list);
                                        } else if (data.rc == 403) {
                                            App.Helpers.error403();
                                            e.success([]);
                                        } else {
                                            e.success([]);
                                        }
                                    },
                                    error: function (data) {
                                        e.success([]);
                                    },
                                });
                            } else {
                                e.success([]);
                            }
                        }
                    },
                }),

                dataTextField: "Phone",
                dataValueld: "PeopleId",
                filter: "contains",
                template: "#:data.Phone#" + " (#:data.I# #:data.O# #:data.F#)",
                select: function (e) {
                    _selectContactAutocomplete(this.dataItem(e.item.index()));
                },
            });
        };


        $form.find(".phone-state").on("click", function (e) {
            e.preventDefault();

            App.Helpers.clearForm($form, params);
            App.Helpers.clearFormAutocomplete($form, $kendoFormContactList);
        });

        _lastnameAutocomplete();
        _phoneAutocomplete();
    },

    /** Init filter handler */
    initFilterHandler: function () {
        $(".child-item-data-container").find(".filter-state").on("click", function (e) {
            e.preventDefault();

            var
                $currentElement = $(this),
                $parent = $currentElement.closest(".child-item-data-container"),
                $filterState = $parent.find(".filter-state"),
                $filterElement = $parent.find(".child-item-data-filter")
            ;

            /** Clear filter */
            $parent.find(".child-item-data-form-elements-element").removeClass("hide");
            $filterElement.val("");

            /** Set filter state */
            $filterState.addClass("hide");
        });


        $(".child-item-data-filter").on("keyup", function (e) {
            var
                $currentElement = $(this),
                $parent = $currentElement.closest(".child-item-data-container"),
                $filterState = $parent.find(".filter-state"),
                currentValue = ($.trim($currentElement.val())).toLocaleLowerCase()
            ;

            /** Clear filter */
            $parent.find(".child-item-data-form-elements-element").removeClass("hide");

            /** Set filter state */
            (currentValue.length > 0) ? $filterState.removeClass("hide") : $filterState.addClass("hide");

            /** Filter data */
            if (currentValue.length > 2) {
                var test = $parent.find(".child-item-data-form-elements-element input")
                    .filter(function () {
                        return $(this).attr('data-label').toLowerCase().indexOf(currentValue) == -1;
                    })
                    .closest(".child-item-data-form-elements-element")
                    .addClass("hide")
                ;
            }
        });
    },


    /** Init city handler */
    initCityHandler: function ($form, successCallback) {
        var
            $formPropertyCity = $form.find("[name=city]"),
            $formPropertyDistrict = $form.find("[name=district]")
        ;

        $formPropertyCity.on("change", function (e, extraData) {
            var
                $this = $(this),
                currentValue = $this.val()
            ;

            /** Remove all not first option */
            $formPropertyDistrict.find("option:not(:first)").remove();

            /** Set new data */
            if (currentValue) {
                kendo.ui.progress($form, true);

                /** Get promise */
                var promise = App.Helpers.getPromiseAjax($form, $this);

                /** Set data */
                promise
                    .done(function (data) {
                        if (data.rc != -1) {
                            $.each(data.List, function (index, district) {
                                var option = '<option id="' + district.Id + '" value="' + district.Value + '">' + district.Title + '</option>';
                                $formPropertyDistrict.find("option:first").after(option);
                            });

                            if ($.isFunction(successCallback)) {
                                successCallback(data, extraData);
                            }
                        } else {
                            alert("Ошибка при получении данных");
                        }
                    })
                    .fail(function (data) {

                    })
                    .always(function (data) {
                        kendo.ui.progress($form, false);
                    })
                ;
            }
        });
    },

    /** Init commercial handler */
    initCommercialHandler: function ($form, params) {
        var $formPropertyCommercional = $form.find("[name=comercial]");
        $formPropertyCommercional.on("change", function (e) {
            var $this = $(this),
                roomsId = "#" + params.roomsId,
                areaId = "#" + params.roomsId,
                areaCommercialOptions = params.areaCommercialOptions,
                areaOptions = params.areaOptions
            ;

            if ($this.prop("checked")) {
                /** Rooms */
                $(roomsId).data("kendoCustomRangeSlider").enable(false);

                /** Square */
                App.Property.initRangeSlider(areaCommercialOptions, true);
            } else {
                /** Rooms */
                $(roomsId).data("kendoCustomRangeSlider").enable(true);

                /** Square */
                App.Property.initRangeSlider(areaOptions, true);
            }
        });
    },


    /** Init child modal handler */
    modalWindowCollection: {},
    modalWindowCollectionProperty: {},
    initChildModalHandler: function ($form) {
        $form.find(".child-item-container").find("input[data-ajax=True]").on("change", function (e, options) {
            var $this = $(this),
                currentValue = $this.val(),
                modalWindowId = $this.attr("data-modal-window-id"),
                $childItemContainer = $this.closest(".child-item-container"),
                $childItemDataContainer = $childItemContainer.find(".child-item-data-container"),
                $childItemProxyData = $childItemContainer.find(".child-item-proxy-data"),
                $childItemDataFormElements = $childItemContainer.find(".child-item-data-form-elements"),
                $childItemStatus = $childItemContainer.find(".child-item-status")
            ;

            /** Init new window if not isset old */
            if (App.Property.modalWindowCollection[modalWindowId] === undefined) {
                App.Property.modalWindowCollection[modalWindowId] = $("#" + modalWindowId).kendoCustomWindow({
                    width: "60%",
                    maxHeight: "500px",
                    modal: true,
                    resizable: false,
                    visible: false,
                    activate: function () {
                        App.Property.modalWindowCollection[modalWindowId].center();
                    },
                    close: function () {
                        /* Get current modal window element */
                        var $currentModalWindowElement = App.Property.modalWindowCollection[modalWindowId].element;

                        if ($currentModalWindowElement && $currentModalWindowElement.length) {
                            /* Get property values */
                            var childFilterValues = (App.Property.modalWindowCollectionProperty[modalWindowId]) ? App.Property.modalWindowCollectionProperty[modalWindowId] : {};

                            /* Clear form */
                            App.Helpers.resetContainer($currentModalWindowElement);

                            /** Fill data */
                            App.Helpers.setFormData(childFilterValues);
                            App.Helpers.fillForm($currentModalWindowElement);
                        }
                    },
                }).data("kendoCustomWindow");
                App.Property.modalWindowCollection[modalWindowId].element.removeClass("hide");
            }

            /** TODO - FIX duplicate request */
            /** Set new data */
            if (!$childItemDataFormElements.children().length) {
                /** Get promise */
                var promise = App.Helpers.getPromiseAjax($form, $this),
                    htmlStr = ""
                ;

                promise
                    .done(function (data) {
                        $.each(data.List, function (index, element) {
                            htmlStr += App.Render.renderChildItem(element, { countItems: data.List.length, index: index });
                        });

                        htmlStr = (htmlStr) ? htmlStr : "Нет данных";
                        if (currentValue == "pay-3") {
                            htmlStr = "";
                            /** Prepare data */
                            var arrInstallment = {},
                                countArrInstallment = 0
                            ;
                            $.each(data.List, function (index, element) {
                                var titleSplit = element.Title.split("|");
                                var newElement = element;
                                newElement["Title"] = titleSplit[0];
                                if (!arrInstallment[titleSplit[1]]) {
                                    arrInstallment[titleSplit[1]] = [];
                                    countArrInstallment += 1;
                                }

                                arrInstallment[titleSplit[1]].push(newElement);
                            });

                            $.each(arrInstallment, function (index, column) {
                                var columnNummer = Math.floor(12 / countArrInstallment);
                                columnNummer = (columnNummer < 2) ? 12 : columnNummer;
                                htmlStr += "<div class='col-md-" + columnNummer + "'>";
                                htmlStr += "<h3>" + index + " комнатная</h3>";

                                $.each(column, function (index, element) {
                                    htmlStr += App.Render.renderChildItem(element, { countItems: column.length, index: index, columnNumber: 12 });
                                });

                                htmlStr += "</div>";
                            });

                            $childItemDataFormElements.html(htmlStr);

                            /** Reinit query for installment */
                            App.Property.modalWindowCollection[modalWindowId].element.find(".child-item-data-form-elements").html(htmlStr);
                        } else {
                            $childItemDataFormElements.html(htmlStr);
                        }

                        /** Fill child data */
                        if (options && options.fillChilData) {
                            /** Fill container data */
                            App.Helpers.fillForm(App.Property.modalWindowCollection[modalWindowId].element);

                            /** Trigger click ok */
                            App.Property.modalWindowCollection[modalWindowId].element.find(".b-form-btn-ok").trigger("click");
                        }
                    })
                    .fail(function (data) {

                    })
                    .always(function (data) {

                    })
                ;
            }

            if ($this.prop("checked")) {
                App.Property.modalWindowCollection[modalWindowId]
                    .title($.trim($this.closest("label").text()))
                    .center()
                    .open(options)
                ;
            } else {
                $childItemProxyData.html("");
                $childItemStatus.html("").addClass("hide");

                /** Clear old data */
                App.Helpers.resetContainer(App.Property.modalWindowCollection[modalWindowId].element);
            }

        });

        $("body").on("click", ".b-form-btn-ok", function (e) {
            e.preventDefault();

            var $this = $(this),
                modalWindowId = $this.attr("data-modal-window-id"),
                childItemContainerId = $this.attr("data-child-item-container-id"),
                $parent = $this.closest(".k-window"),
                $childItemDataFormElements = $parent.find(".child-item-data-form-elements"),
                $childItemContainer = $("#" + childItemContainerId),
                $childItemProxyData = $childItemContainer.find(".child-item-proxy-data"),
                $childItemStatus = $childItemContainer.find(".child-item-status"),

                statusMessage = []
            ;

            /** Clear modal window porperty collection */
            App.Property.modalWindowCollectionProperty[modalWindowId] = {};

            /** Get radio && checkbox data data */
            $childItemDataFormElements.find("input[type=radio], input[type=checkbox]").each(function (index, elem) {
                var $elem = $(elem);

                if ($elem.prop("checked")) {
                    var label = $.trim($elem.attr("data-label"));

                    /** Set new value */
                    if (!App.Property.modalWindowCollectionProperty[modalWindowId][$elem.attr("name")]) {
                        App.Property.modalWindowCollectionProperty[modalWindowId][$elem.attr("name")] = [];
                    }
                    App.Property.modalWindowCollectionProperty[modalWindowId][$elem.attr("name")].push($elem.val());

                    if (label) {
                        statusMessage.push(label);
                    }
                }
            });

            /** Get select option data */
            $childItemDataFormElements.find("select option:selected").each(function (index, elem) {
                var $elem = $(elem);
                var label = $.trim($elem.attr("data-label"));

                /** Set new value */
                if (!App.Property.modalWindowCollectionProperty[modalWindowId][$elem.attr("name")]) {
                    App.Property.modalWindowCollectionProperty[modalWindowId][$elem.attr("name")] = [];
                }
                App.Property.modalWindowCollectionProperty[modalWindowId][$elem.attr("name")].push($elem.val());


                if (label) {
                    statusMessage.push(label);
                }
            });

            /** Get text && textarea data */
            $childItemDataFormElements.find("input[type=text], textarea").each(function (index, elem) {
                var $elem = $(elem);

                var label = $.trim($elem.val());

                if (label) {
                    statusMessage.push(label);
                }
            });

            /** Set status */
            $.each(statusMessage, function (index, element) {
                if (element.length > 15) {
                    statusMessage[index] = element.substr(0, 15) + "...";
                }
            });

            if (statusMessage.length) {
                /** Set proxy data  */
                $childItemProxyData.html($childItemDataFormElements.clone().children());

                /** Set status */
                $childItemStatus.html(statusMessage.join("; ")).removeClass("hide");
            } else {
                $childItemProxyData.html("");
                $childItemStatus.html("").addClass("hide");
            }

            /** Close child window */
            App.Property.modalWindowCollection[modalWindowId].close();
        });

        /** Handler child items */
        $form.find(".child-item-container .child-item-status").on("click", function (e) {
            e.preventDefault();

            var $this = $(this),
                modalWindowId = $this.attr("data-modal-window-id"),
                $childItemContainer = $this.closest(".child-item-container")
            ;

            App.Property.modalWindowCollection[modalWindowId]
                .title($.trim($this.closest("label").text()))
                .center()
                .open()
            ;
        });
    },

    /** Init price slider */
    initPriceSlider: function ($form, options, reinit) {
        var reinit = (reinit === true) ? true : false,
            $elem = $("#" + options.id),
            $elemFrom = $("#" + options.id + "-from"),
            $elemTo = $("#" + options.id + "-to"),
            elemName = $elem.attr("data-property-name")
        ;

        /** Reinit */
        if (reinit) {
            $elem.data("kendoCustomRangeSlider").reinit(options);
        } else {
            var kendoWidgetParams = {
                min: options.min,
                max: options.max,
                smallStep: options.smallStep,
                //largeStep: options.largeStep,
                change: function (e) {
                    if (options.callbackOnChange) {
                        options.callbackOnChange(e);
                    }

                    $elemFrom.val(e.values[0]);
                    $elemTo.val(e.values[1]);
                },
            };

            /** Create new */
            $elem.kendoCustomRangeSlider(kendoWidgetParams);
        }

        /** Set value */
        $elemFrom.val(options.min);
        $elemTo.val(options.max);

        /** Blur from */
        $elemFrom.on("blur", function (e) {
            var currentValueFrom = parseInt($.trim($elemFrom.val()), 10),
                currentValueTo = parseInt($.trim($elemTo.val()), 10)
            ;

            if (currentValueFrom < options.min) {
                alert("Ошибка, текущее значение меньше минимального");
                $elemFrom.focus();
            } else if (currentValueFrom > options.max) {
                alert("Ошибка, текущее значение больше максимального");
                $elemFrom.focus();
            } else if (currentValueFrom > currentValueTo) {
                alert("Ошибка, текущее значение больше правой границы");
                $elemFrom.focus();
            } else {
                $elem.data("kendoCustomRangeSlider").updateValue([currentValueFrom, currentValueTo], true);
            }
        });

        /** Blur to */
        $elemTo.on("blur", function (e) {
            var currentValueFrom = parseInt($.trim($elemFrom.val()), 10),
                currentValueTo = parseInt($.trim($elemTo.val()), 10)
            ;

            if (currentValueTo > options.max) {
                alert("Ошибка, текущее значение больше максимального")
                $elemTo.focus();
            } else if (currentValueTo < options.min) {
                alert("Ошибка, текущее значение меньше минимального");
                $elemTo.focus();
            } else if (currentValueTo < currentValueFrom) {
                alert("Ошибка, текущее значение меньше левой границы");
                $elemTo.focus();
            } else {
                $elem.data("kendoCustomRangeSlider").updateValue([currentValueFrom, currentValueTo], true);
            }
        });

        /** Set value*/
        if (options.defMin && options.defMax) {
            $elem.data("kendoCustomRangeSlider").updateValue([options.defMin, options.defMax], true);
        }

        return $elem.data("kendoCustomRangeSlider");
    },

    /** Init kendo range slider */
    initRangeSlider: function (options, reinit) {
        var reinit = (reinit === true) ? true : false,
            $elem = $("#" + options.id),
            elemName = $elem.attr("data-property-name")
        ;

        /** Reinit */
        if (reinit) {
            $elem.data("kendoCustomRangeSlider").reinit(options);
        } else {
            var kendoWidgetParams = {
                min: options.min,
                max: options.max,
                smallStep: options.smallStep,
                largeStep: options.largeStep,
            };

            /** TODO - check callback is function */
            if (options.callbackOnChange) {
                kendoWidgetParams.change = options.callbackOnChange;
            }

            if (options.callbackOnSlide) {
                kendoWidgetParams.slide = options.callbackOnSlide;
            }

            /** Create new */
            $elem.kendoCustomRangeSlider(kendoWidgetParams);
        }

        /** Set value*/
        if (options.defMin && options.defMax) {
            $elem.data("kendoCustomRangeSlider").updateValue([options.defMin, options.defMax], true);
        }


        return $elem.data("kendoCustomRangeSlider");
    },

    /** Init contact list grid */
    initContactListGrid: function ($formContactList) {
        var contactTypes = [{ value: 1, text: "Тел." }, { value: 2, text: "Email" }];
        var contactListDataSourceSchema = {
            model: {
                id: "ItemId",
                fields: {
                    ItemId: { editable: false, nullable: true },
                    ContactTypeId: { validation: { required: true }, type: "number", defaultValue: 1 },
                    ItemValue: { validation: { required: false }, type: "string" },
                    ItemDesc: { validation: { required: false }, type: "string" }
                }
            }
        };
        var $kendoFormContactList = $formContactList.kendoGrid({
            dataSource: {
                schema: contactListDataSourceSchema,
                data: []
            },
            columns: [
                { field: "ContactTypeId", values: contactTypes, title: "Тип", width: "80px" },
                { field: "ItemValue", title: "Контакт" },
                { field: "ItemDesc", title: "Имя" },
                { command: { className: "btn-destroy", name: "destroy", text: "" } }
            ],

            editable: true,
            scrollable: false,
            edit: function (event) {
                this.element.find(".k-input").on("keydown", function (e) {
                    if ([13, 40].indexOf(e.keyCode) !== -1) {
                        $(this).blur();
                        //$kendoFormContactList.addRow();

                        var dataSource = $kendoFormContactList.dataSource;
                        var total = dataSource.data().length;
                        dataSource.insert(total, {});
                        dataSource.page(dataSource.totalPages());
                        $kendoFormContactList.editRow($kendoFormContactList.tbody.children().last());
                    }
                });
            }
        }).data("kendoGrid");
        $kendoFormContactList.addRow();

        return $kendoFormContactList;
    },

    initWorkHouseDateTime: function ($element, params) {
        var _prepareHours = function (str) {
            return parseInt($.trim(str.replace(":", "")), 10);
        };

        /** Get data */
        var params = (params && $.isPlainObject(params)) ? params : {},
            workHours = ((params.workHours) && ($.isArray(params.workHours)) && (params.workHours.length == 2)) ? params.workHours : ["8:00", "20:00"],
            minDate = ((params.minDate) && ($.type(params.minDate) === "date")) ? params.minDate : moment().hours(0).minutes(0).seconds(0).toDate(),
            maxDate = ((params.maxDate) && ($.type(params.maxDate) === "date")) ? params.maxDate : moment().hours(0).minutes(0).seconds(0).add(6, "months").toDate()
        ;

        /** Prepare hours */
        $.each(workHours, function (index, element) {
            workHours[index] = _prepareHours(element);
        });

        /** Init kendo */
        $element.kendoDateTimePicker({
            min: minDate,
            max: maxDate,
            open: function (e) {
                if (e.view === "time") {
                    e.sender.timeView.list.find(".k-list .k-item").each(function (index, current) {
                        var $current = $(current),
                            currentHours = _prepareHours($current.text())
                        ;


                        if (currentHours < workHours[0] || currentHours > workHours[1]) {
                            $current.hide();
                        }
                    });
                }
            }
        });
    },

    initCurrency: function ($element, optionts) {
        var options = ($.isPlainObject(optionts)) ? optionts : {},
            format = (options.format) ? options.format : "c0",
            step = (options.step) ? options.step : 0,
            min = (options.min) ? options.min : 0
        ;

        var result = $element.kendoNumericTextBox({
            culture: "ru-RU",
            format: format,
            step: step,
            min: min,
        }).data("kendoNumericTextBox");

        return result;
    },

    initProfileWindow: function () {
        if (!$("#kendo-modal-window").length) {
            $("<div id='kendo-modal-window' class='b-js-generated' />").appendTo(document.body)
        }

        var $kendoModalWindow = $("#kendo-modal-window").data("kendoWindow");

        if (!$kendoModalWindow) {
            $kendoModalWindow = $("#kendo-modal-window").kendoWindow({
                width: "30%",
                modal: true,
                resizable: false,
                visible: false,
                open: function () {
                    kendo.ui.progress($("body"), true);
                },
                refresh: function () {
                    kendo.ui.progress($("body"), false);
                    $kendoModalWindow.center();
                },
            }).data("kendoWindow");
        }

        return $kendoModalWindow;
    },
};