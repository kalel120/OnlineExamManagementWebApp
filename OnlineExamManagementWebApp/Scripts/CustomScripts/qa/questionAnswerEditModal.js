// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const $EXAM_ID = $.trim($("#js_examId").val());
        const $QUESTION_ID = $("#js-editModal-hidden-qId");

        const qoEditModal = $("#js-modal-editQo");
        const qoEditModalForm = $("#js-modal-editQo-form");
        const qoEditModalFormInputs = $("#js-modal-editQo-form :input");
        const qoEditOrderTextBox = $("#js-modal-editQo-order");
        const qoEditMarksTextBox = $("#js-modal-editQo-marks");
        const qoEditDescTextBox = $("#js-modal-editQo-qText");
        const qoEditAddOptionTextBox = $("#js-modal-editQo-oText");
        const qoEditSingleRadioBtn = $("#js-modal-editQo-oType-single");
        const qoEditMultiRadioBtn = $("#js-modal-editQo-oType-multiple");
        const editQoSubmitBtn = $("#js-modal-editQo-Submit");
        const editQoCloseBtn = $(".js-modal-editQo-close");
        const qoEditModalTbl = $("#js-tbl-editOptions-modal");
        const qoEditModalTblBody = $("#js-tbl-editOptionModal-tbody");
        const addOptionDiv = $(".js-div-addOption-editQoModal");


        const resetModalState = function () {
            editQoSubmitBtn.hide();
            addOptionDiv.hide();
        }

        resetModalState();
        /*END*/

        /** Jobs when modal pops */
        qoEditModal.on("shown.bs.modal", function () {
            if (qoEditModalTblBody.find("tr").length === 4) {
                return;
            }
            addOptionDiv.show();
        });
        /**END*/

        /** Jobs when modal got hidden */
        // Get updated question from server for this Question
        const getUpdatedQuestion = async function () {
            try {
                let response = await $.ajax({
                    url: `/QuestionAnswer/GetQuestionsByExamId?id=${$EXAM_ID}`,
                    contentType: "json",
                    dataType: "json"
                });
                return response;

            } catch (error) {
                // Enable this before going live
                //if (error.status === 500) {
                //    window.location = "/Error/Error500";
                //}
            }
        };

        qoEditModal.on("hidden.bs.modal", function () {
            $(this).find("form").trigger("reset");
            resetModalState();
            qoEditModalTblBody.empty();

            // Refresh question jquery datatable for this exam
            getUpdatedQuestion()
                .then(function (resData) {
                    $('#tbl-questions').DataTable().clear().rows.add(resData).draw();
                });

        });

        /**END */

        /** On any input change, will trigger an event to enable disable save button */
        qoEditModalFormInputs.on("keyup change", function () {
            editQoSubmitBtn.show();
        });

        qoEditModalTbl.on("change", function () {
            editQoSubmitBtn.show();
        });
        /**END */

        /** bootstrap alerts **/
        const showAndDismissWarningAlert = function (message) {
            let html = `<div class="alert alert-warning" role="alert" id="js-editQo-alert-warning">`;
            html += `<button type="button" class="close" data-dismiss="alert" aria-label="Close" data-form-type=""><span aria-hidden="true">×</span></button>`;
            html += `<strong>Warning!</strong> ${message} </div>`;

            $("#js-editQo-alert").append(html);
            $("#js-editQo-alert-warning").addClass("animated bounceIn");

            setTimeout(function () {
                $("#js-editQo-alert-warning").addClass("animated bounceOut").remove();
            }, 10000);
        };

        const showAndDismissSuccessAlert = function (message) {
            let html = `<div class="alert alert-success" role="alert" id="js-editQo-alert-success">`;
            html += `<button type="button" class="close" data-dismiss="alert" aria-label="Close" data-form-type=""><span aria-hidden="true">×</span></button>`;
            html += `<strong>Success!</strong> ${message} </div>`;

            $("#js-editQo-alert").append(html);
            $("#js-editQo-alert-success").addClass("animated tada");

            setTimeout(function () {
                $("#js-editQo-alert-success").addClass("animated fadeOut").remove();
            }, 10000);
        };

        const showAndDismissErrorAlert = function (message) {
            let html = `<div class="alert alert-danger" role="alert" id="js-editQo-alert-error">`;
            html += `<button type="button" class="close" data-dismiss="alert" aria-label="Close" data-form-type=""><span aria-hidden="true">×</span></button>`;
            html += `<strong>Error!</strong> ${message} </div>`;

            $("#js-editQo-alert").append(html);
            $("#js-editQo-alert-error").addClass("animated flash");

            setTimeout(function () {
                $("#js-editQo-alert-error").addClass("animated flash").remove();
            }, 10000);
        };
        /**END**/

        /** validation **/
        const validation = function () {
            return qoEditModalForm.validate({
                errorClass: "text-danger",
                errorElement: "div",

                rules: {
                    "Marks": {
                        required: true,
                        digits: true
                    },
                    "QuestionDescription": {
                        required: true,
                        minlength: 10,
                        maxlength: 200
                    },
                    "OptionDescription": {
                        required: true,
                        minlength: 1,
                        maxlength: 50
                    }
                },
                messages: {
                    "Marks": {
                        required: "Insert Question Marks",
                        digits: "Only positive integer are allowed"
                    },
                    "QuestionDescription": {
                        required: "Enter Question",
                        minlength: "Option can not be less than 10 character",
                        maxlength: "Option can not exceed more than 200 characters "
                    },
                    "OptionDescription": {
                        required: "Enter Option",
                        minlength: "Option can not be less than 1 character",
                        maxlength: "Option can not exceed more than 50 characters "
                    }
                },
                errorPlacement: function (error, element) {
                    if (element.parent(".input-group > div").length) {
                        element.parent(".input-group > div").append(error);
                    }
                    element.parent(".form-group > div").append(error);
                },

                highlight: function (element) {
                    $(element).closest(".form-group").removeClass("has-success has-error")
                        .addClass("has-error");
                },

                unhighlight: function (element) {
                    $(element).closest(".form-group").removeClass("has-error");
                },

                success: function (element) {
                    element.closest(".form-group").removeClass("has-success has-error");
                }
            }).form();
        };
        /*END*/

        /** Popup question options edit modal **/
        const getRowOfQuestionTableAsObject = function (row) {
            let content = {
                QuestionId: row.find("td:eq(5)").children(".js-qoEditModalPopup").attr("data-question-id"),
                Serial: $.trim(row.find("td:eq(0)").text()),
                Marks: $.trim(row.find("td:eq(1)").text()),
                Description: $.trim(row.find("td:eq(2)").text()),
                OptionType: $.trim(row.find("td:eq(3)").text())
            };

            return content;
        };

        const getOptionsByQuestionId = function (questionId) {
            return $.ajax({
                type: "GET",
                url: `/QuestionAnswer/GetOptionsByQuestionId?id=${questionId}`,
                dataType: "json"
            });
        };

        const bindNewOptionToTbl = function (data) {
            if (!data) { return; }

            let html = `<tr>`;
            html += `<td>${data.Order}</td>`;
            html += `<td> ${data.Description}</td>`;

            if (qoEditSingleRadioBtn.prop("checked")) {
                if (data.IsMarkedAsAnswer) {
                    html += `<td><input type="radio" value=${data.OptionId} name="OptionEditModalAnsSelect" checked/> </td>`;
                } else {
                    html += `<td><input type="radio" value=${data.OptionId} name="OptionEditModalAnsSelect"/> </td>`;
                }

            }

            if (qoEditMultiRadioBtn.prop("checked")) {
                if (data.IsMarkedAsAnswer) {
                    html += `<td><input type = "checkbox" value=${data.OptionId} name="OptionEditModalAnsSelect" checked="checked"/></td>`;
                } else {
                    html += `<td><input type = "checkbox" value=${data.OptionId} name="OptionEditModalAnsSelect"/></td>`;
                }
            }

            html += `<td><a href="#" data-option-id="${data.OptionId}" class="js-editOptions-modal-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a>`;

            if (!data.OptionId) {
                html += `<a href="#" data-option-id="" class="js-editOptions-modal-tbl-saveOption btn btn-success"><i class="avoid-clicks fa fa-check-square"> Save </i> </a>`;
            }

            html += `</td></tr>`;
            qoEditModalTbl.append(html);
        };

        const bindToEditQoModal = function (data) {
            qoEditOrderTextBox.val(data.Serial);
            qoEditMarksTextBox.val(data.Marks);
            qoEditDescTextBox.val(data.Description);

            if (data.OptionType === qoEditSingleRadioBtn.val()) {
                qoEditSingleRadioBtn.prop("checked", true);
                qoEditMultiRadioBtn.prop("checked", false);
            }

            if (data.OptionType === qoEditMultiRadioBtn.val()) {
                qoEditMultiRadioBtn.prop("checked", true);
                qoEditSingleRadioBtn.prop("checked", false);
            }
        };

        $(document).on("click", ".js-qoEditModalPopup", async function (event) {
            try {
                let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));
                bindToEditQoModal(tableRow);
                $QUESTION_ID.val(tableRow.QuestionId);

                // asynchronously get options data from server and dynamically build html options table
                let options = await getOptionsByQuestionId(tableRow.QuestionId);
                options.forEach(bindNewOptionToTbl);

                qoEditModal.modal("toggle");
            } catch (exception) {
                if (exception.status === 404) {
                    window.location = "/Error/Error404";
                } else {
                    console.log(exception);
                }
            }
        });
        /*END*/

        /*** Remove Option from edit modal and db and re-sequence option serial */
        const removeOptionOnServer = function (optionId, examId) {
            let optionToRemove = {
                OptionId: optionId,
                ExamId: examId
            };

            return $.ajax({
                type: "PUT",
                url: `/QuestionAnswer/RemoveAnOption`,
                dataType: "json",
                data: optionToRemove
            })
                .fail(function (res, textStatus, errorThrown) {
                    console.log(res);
                    console.log(textStatus);
                    console.log(errorThrown);
                });
        };

        const reOrderOptionsOnServer = function (options, examId, questionId) {
            return $.ajax({
                type: "PUT",
                url: `/QuestionAnswer/ReOrderOptionsOnRemove?examId=${examId}&questionId=${questionId}`,
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(options)
            })
                .fail(function (res, textStatus, errorThrown) {
                    console.log(res);
                    console.log(textStatus);
                    console.log(errorThrown);
                });
        };

        const getReOrderedOptions = function () {
            if (qoEditModalTblBody.children().length === 0) {
                return null;
            }

            let reOrderedOptions = new Array();
            qoEditModalTblBody.find("tr").each(function (index, element) {
                let optionId = $(element).find("td:eq(3) > a").data("option-id");
                if (!optionId) { return; }

                let option = {
                    Order: index + 1,
                    OptionId: $(element).find("td:eq(3) > a").attr("data-option-id")
                };
                reOrderedOptions.push(option);
            });

            return reOrderedOptions;
        };

        const removeRowAndReOrderOnClient = function (tr) {
            tr.remove();

            qoEditModalTblBody.find("tr").each(function (index, element) {
                $(element).find("td:eq(0)").html(index + 1);
            });
        };

        $(document).on("click", ".js-editOptions-modal-remove-option", function (event) {
            bootbox.confirm("Are you sure?", async function (confirmed) {
                if (!confirmed) return;

                let tr = $(event.target).closest("tr");
                let optionId = tr.find("td:eq(3) > a").data("option-id");

                try {
                    // If optionId doesn't exists(returns null) then remove only from options table
                    if (!optionId) {
                        removeRowAndReOrderOnClient(tr);
                    }
                    else {
                        removeRowAndReOrderOnClient(tr);

                        let isRemoved = await removeOptionOnServer(optionId, $EXAM_ID);
                        if (!isRemoved) { return; }

                        let reOrderedOptions = getReOrderedOptions();
                        // if the last option gets removed then don't reorder
                        if (!reOrderedOptions) { return; }

                        await reOrderOptionsOnServer(reOrderedOptions, $EXAM_ID, $QUESTION_ID.val());

                        showAndDismissWarningAlert("Option has removed and re-ordered");
                    }
                    addOptionDiv.show();

                } catch (error) {
                    if (error.status === 500) {
                        window.location = "/Error/Error500";
                    }
                }
            });
        });
        /*** END */

        /***  Add option if option count is less than 4 */
        const getSingleOptionToBindOnHtmlTbl = function () {
            return {
                Order: qoEditModalTblBody.find("tr").length + 1,
                Description: $.trim(qoEditAddOptionTextBox.val()),
                QuestionId: $QUESTION_ID.val(),
                ExamId: $EXAM_ID,
                OptionId: null
            }
        };

        const getSingleOptionToSaveOnDb = function (tr) {
            return {
                SerialNo: tr.find("td:eq(0)").text(),
                OptionText: tr.find("td:eq(1)").text(),
                IsCorrectAnswer: tr.find("td:eq(2)").find("input[type='checkbox']").prop("checked"),
                ExamId: $EXAM_ID,
                QuestionId: $QUESTION_ID.val()
            }
        };

        const saveSingleOptionToDb = function (option) {
            return $.ajax({
                type: "POST",
                url: `/QuestionAnswer/SaveSingleOption`,
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(option)
            });
        };

        const isOptionTextExistsOnTable = function (newOptionText) {
            let result = false;

            qoEditModalTblBody.find("tr").each(function (index, element) {
                let tblOptionText = $.trim($(element).find("td:eq(1)").text().toLowerCase());
                if (newOptionText === tblOptionText) {
                    result = true;
                    return;
                }
            });
            return result;
        };

        $(document).on("click", "#js-btn-editOptionModal-AddOption", function (event) {
            if (!validation()) { return; }

            let option = getSingleOptionToBindOnHtmlTbl();

            if (isOptionTextExistsOnTable(option.Description.toLowerCase())) {
                bootbox.alert("Option Description already exists on this table");
                return;
            }

            bindNewOptionToTbl(option);

            // hide add option button based on table row count
            if (qoEditModalTblBody.find("tr").length === 4 && addOptionDiv.is(":visible")) {
                addOptionDiv.hide();
            }
        });
        /*** END */

        /** Save newly added option to db **/
        $(document).on("click", ".js-editOptions-modal-tbl-saveOption", async function (event) {
            let tr = $(event.target).closest("tr");
            let singleOptionToSaveDb = getSingleOptionToSaveOnDb(tr);

            try {
                // save single option to db and remove save button
                let resultOfSave = await saveSingleOptionToDb(singleOptionToSaveDb);
                if (!resultOfSave) {
                    return;
                }

                // asynchronously get options data from server and dynamically build html options table
                qoEditModalTblBody.empty();
                let options = await getOptionsByQuestionId($QUESTION_ID.val());
                options.forEach(bindNewOptionToTbl);

                // display alert
                showAndDismissSuccessAlert("New Option Saved");
            }
            catch (error) {
                // Enable this before going live
                //if (error.status === 500) {
                //    window.location = "/Error/Error500";
                //}
            }
        });
        /***END*/

        /** Change option type selection **/
        // Update Answer selection type of Question by exam and question id
        const updateAnswerSelectionType = async function (optionType) {
            let reqData = {
                OptionType: optionType,
                ExamId: $EXAM_ID,
                QuestionId: $QUESTION_ID.val()
            };

            try {
                let response = await $.ajax({
                    type: "PUT",
                    url: "/QuestionAnswer/UpdateOptionTypeOfQuestion",
                    dataType: "json",
                    data: reqData
                });

                return response;
            } catch (error) {
                if (error.status === 500) {
                    window.location = "/Error/Error500";
                }
            }
        };

        $(document).on("change", "input[name = 'OptionTypeEditModal']", function (event) {
            bootbox.confirm({
                size: "small",
                message: "Are You Sure? This will clear out already selected correct answers",
                callback: async function (confirmed) {
                    if (!confirmed) {
                        // Reset Option type selection radio buttons and exit event
                        $("input[name = 'OptionTypeEditModal']").not(':checked').prop("checked", true);
                        return;
                    }

                    let resultAfterUpdate = await updateAnswerSelectionType($("input[name='OptionTypeEditModal']:checked").val());
                    if (!resultAfterUpdate) {
                        return;
                    }

                    // Clear options table. Then get options data from server and dynamically build options table
                    qoEditModalTblBody.empty();
                    let options = await getOptionsByQuestionId($QUESTION_ID.val());
                    options.forEach(bindNewOptionToTbl);

                    showAndDismissWarningAlert("Correct Answer Type of this question has modified");
                }
            });

        });
        /**END**/

        /** Show unsaved state if correct answer selection is modified **/
        const isRemovedUpdateCorrectAnsBtn = function (optionType, thisUpdateAnsClass) {
            let result = false;
            const tbodyUpdateAnsClass = qoEditModalTblBody.find(".js-editOptions-modal-tbl-updateAnswer");

            // for single answer option type
            if (optionType === "single") {
                if (tbodyUpdateAnsClass.length !== 0) {
                    tbodyUpdateAnsClass.remove();
                    result = true;
                }
            }

            // for multiple answer option type
            if (optionType === "multiple") {
                if ((thisUpdateAnsClass.length >= 1)) {
                    thisUpdateAnsClass.remove();
                    result = true;
                }
            }

            return result;
        };

        const appendUpdateCorrectAnsBtn = function (optionId, eventTarget) {
            const html = `<a href="#" data-option-id="${optionId}" class="js-editOptions-modal-tbl-updateAnswer btn btn-primary"><i class="avoid-clicks fa fa-database"> Update Answer </i> </a>`;

            eventTarget.closest("tr").find("td:eq(3)").append(html);
        };

        const handleUpdateAnsBtnForSingleOptionType = function (thisUpdateAnsClass, optionId, eventTarget) {
            // For single, remove all update button from table body, then append update answer button
            isRemovedUpdateCorrectAnsBtn("single", thisUpdateAnsClass);
            appendUpdateCorrectAnsBtn(optionId, eventTarget);
        };

        const handleUpdateAnsBtnForMultiOptionType = function (thisUpdateAnsClass, optionId, eventTarget) {
            // For multiple, remove if update button already exits, then append update answer button
            if (!isRemovedUpdateCorrectAnsBtn("multiple", thisUpdateAnsClass)) {
                appendUpdateCorrectAnsBtn(optionId, eventTarget);
            }
        };


        // Hacky way to get previously selected correct answer.
        let previouslyChecked;
        $(document).on("focus", "input[name='OptionEditModalAnsSelect']", function () {
            previouslyChecked = $("input[name='OptionEditModalAnsSelect']:checked").val();
        });

        $(document).on("change", "input[name='OptionEditModalAnsSelect']", function (event) {
            if ($(event.target).closest("tr").find("td:eq(3) > a").hasClass("js-editOptions-modal-tbl-saveOption")) {
                $(this).prop("checked", false);

                // Compare options checkboxes with previously checked one and set it's status to 'checked'
                $("input[name='OptionEditModalAnsSelect']").each(function () {
                    if (typeof previouslyChecked === "undefined") {
                        return;
                    }

                    if ($(this).val() === previouslyChecked) {
                        $(this).prop("checked", true);
                    }
                });

                showAndDismissErrorAlert("This option is not saved yet!");
                return;
            }
            let eventTarget = $(event.target);
            let thisUpdateAnsClass = eventTarget.closest("tr").find(".js-editOptions-modal-tbl-updateAnswer");

            if (qoEditSingleRadioBtn.prop("checked")) {
                handleUpdateAnsBtnForSingleOptionType(thisUpdateAnsClass, eventTarget.val(), eventTarget);
                return;
            }
            if (qoEditMultiRadioBtn.prop("checked")) {
                handleUpdateAnsBtnForMultiOptionType(thisUpdateAnsClass, eventTarget.val(), eventTarget);
                return;
            }
        });
        /** END **/

        /** Save updated correct answer **/
        const isCorrectAnsUpdatedOnServer = async function (dataToUpdate) {
            try {
                let response = await $.ajax({
                    type: "PUT",
                    url: "/QuestionAnswer/UpdateCorrectAnsOfOption",
                    dataType: "json",
                    data: dataToUpdate
                });

                return response;
            } catch (error) {
                return false;
                // Enable this before going live
                //if (error.status === 500) {
                //    window.location = "/Error/Error500";
                //}
            }
        };

        const isCorrectAnsUpdated = async function (eventTarget) {
            try {
                let optionId = eventTarget.find("td:eq(2)").children("input[name='OptionEditModalAnsSelect']").val();
                let isCorrectAns = eventTarget.find("td:eq(2)").children("input[name='OptionEditModalAnsSelect']").prop("checked");

                // build object to send to server
                let optionToUpdate = {
                    OptionId: optionId
                    , QuestionId: $QUESTION_ID.val()
                    , ExamId: $EXAM_ID
                    , OptionType: $("input[name='OptionTypeEditModal']:checked").val()
                    , IsCorrectAnswer: isCorrectAns
                };
                return await isCorrectAnsUpdatedOnServer(optionToUpdate);
            } catch (error) {
                if (error.status === 500) {
                    window.location = "/Error/Error500";
                }
            }
        };

        $(document).on("click", ".js-editOptions-modal-tbl-updateAnswer", function (event) {
            let resultAfterUpdate = isCorrectAnsUpdated($(event.target).closest("tr"));

            resultAfterUpdate.then(function (resValue) {
                $(event.target).remove();

                if (!resValue) {
                    showAndDismissWarningAlert("Update Failed. Possible Reason: This is already marked as correct answer");
                } else {
                    showAndDismissSuccessAlert("Correct Answer Updated");
                }
            });
        });
        /** END **/

        /** Save change button actions **/
        const anyUnsavedOption = function () {
            let isUnsaved = false;

            qoEditModalTblBody.find("tr").each(function (index, element) {
                if ($(element).find("td:eq(3) > a").hasClass("js-editOptions-modal-tbl-saveOption")) {
                    isUnsaved = true;
                    return;
                }
            });
            return isUnsaved;
        };

        const anyUncheckedCorrectAns = function () {
            let isUnchecked = false;

            let notCheckedLength = $("input[name='OptionEditModalAnsSelect']").not(":checked").length;
            let checkBoxCount = $("input[name='OptionEditModalAnsSelect']").length;

            if (notCheckedLength === checkBoxCount) {
                isUnchecked = true;
            }
            return isUnchecked;
        };

        const anyUpdatePending = function () {
            let isUnsaved = false;

            qoEditModalTblBody.find("tr").each(function (index, element) {
                if ($(element).find("td:eq(3) > a").hasClass("js-editOptions-modal-tbl-updateAnswer")) {
                    isUnsaved = true;
                    return;
                }
            });
            return isUnsaved;
        };

        const isQuestionTextAlreadyExists = function (newQuestionText) {
            let result = false;
            newQuestionText = newQuestionText.toLowerCase();

            $("#tbl-questions tbody").find("tr").each(function (index, element) {
                let tblQuestionText = $.trim($(element).find("td:eq(2)").text().toLowerCase());

                if (newQuestionText === tblQuestionText) {
                    result = true;
                    return;
                }
            });
            return result;
        };

        const getQuestionDetailsFromModal = function () {
            const question = {
                ExamId: $EXAM_ID,
                QuestionId: $QUESTION_ID.val(),
                Marks: $.trim($("#js-modal-editQo-marks").val()),
                Description: $.trim($("#js-modal-editQo-qText").val())
            };

            if (isQuestionTextAlreadyExists(question.Description)) {
                return null;
            }
            return question;
        };

        const saveUpdatedQuestionOnServer = function (question) {
            return $.ajax({
                type: "PUT",
                url: "/QuestionAnswer/UpdateQuestion",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(question)
            });
        };
        editQoSubmitBtn.on("click", async function () {
            if (anyUncheckedCorrectAns()) {
                bootbox.alert("Still unanswered option left");
                return;
            }

            if (anyUnsavedOption()) {
                bootbox.alert("Still unsaved options left");
                return;
            }

            if (anyUpdatePending()) {
                bootbox.alert("Still pending update");
                return;
            }

            if (qoEditModalTblBody.find("tr").length !== 4) {
                bootbox.alert("Options are less than 4");
                return;
            }

            let questionToUpdate = getQuestionDetailsFromModal();
            if (!questionToUpdate) {
                showAndDismissErrorAlert("This question already exists!");
                return;
            }

            // save on db
            try {
                let resultAfterUpdate = await saveUpdatedQuestionOnServer(questionToUpdate);
                console.log(resultAfterUpdate);

            } catch (error) {
                showAndDismissErrorAlert(`Error ${error.status} occurred`);
                console.log(error);
            }
        });
        /*END*/

        /** Modal close button actions*/
        editQoCloseBtn.on("click", function () {
            if (anyUncheckedCorrectAns()) {
                bootbox.alert("Still unanswered option left");
                editQoCloseBtn.removeAttr("data-dismiss");
                return;
            }

            if (anyUnsavedOption()) {
                bootbox.alert("Still unsaved option left");
                editQoCloseBtn.removeAttr("data-dismiss");
                return;
            }


            editQoCloseBtn.attr("data-dismiss", "modal");
            //window.location.reload();
        });
        /**END*/
    });
})(jQuery);