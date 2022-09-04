// IIFE to wrap jQuery code

(($) => {
    $(() => {
        //start

        /**
         * Initialize
         */
        let loadingSpinner = $('#loading');
        loadingSpinner.hide();

        const examId = $('#js_examId').val();
        const middleRowPanel = $('#qa_entry_middleRow');
        const btnToggleQaSection = $('#btn-toggle-qaSection');

        middleRowPanel.hide();

        $('#hide-section').on('click', function () {
            middleRowPanel.hide();
        });

        btnToggleQaSection.on('click', function () {
            middleRowPanel.show();
        });

        let btnQASubmit = $('#btn_qaSubmit');
        btnQASubmit.hide();
        /**END */

        /**
         * Validation
         */
        const qaEntryFormValidation = () => {
            const qaEntryForm = $("#form_qaEntry");

            return qaEntryForm.validate({
                errorClass: "text-danger",
                errorElement: "div",

                rules: {
                    "Serial": {
                        required: true
                    },
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
                    "Serial": {
                        required: "Insert Question Order"
                    },
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
        }
        /**
         * Validation END
         */

        /**
         * DataTable section
         */
        const tblQuestions = $('#tbl-questions').DataTable({
            "ajax": {
                "beforeSend": () => { loadingSpinner.show(); },
                "url": `/QuestionAnswer/GetQuestionsByExamId?id=${examId}`,
                "contentType": "applicaiton/json",
                "data": function (d) {
                    return JSON.stringify(d);
                },
                "complete": () => { loadingSpinner.hide(); },
                "dataSrc": ""
            },
            "columns": [
                { "data": "Serial" },
                { "data": "Marks" },
                { "data": "Description" },
                { "data": "OptionType" },
                {
                    "data": "QuestionOption",
                    "render": function (data, row) {
                        return data.length;
                    }
                },
                {
                    "data": null,
                    "render": function (data, row) {
                        return `<a href="#" class="btn btn-primary" data-org-id="${data.QuestionId}"><i class="avoid-clicks fa fa-folder"> View</i></a>
                                 <a href="#" class="btn btn-info" data-org-id="${data.QuestionId}"><i class="avoid-clicks fa fa-pencil"> Edit</i></a>
                                 <a href="#" class="btn btn-danger" data-org-id="${data.QuestionId}"><i class="avoid-clicks fa fa-trash-o"> Delete</i></a>`;
                    }
                }
            ]
            //"processing": true,
            //"language": {
            //    "loadingRecords": "&nbsp;",
            //    "processing": '<div id="loading"></div>'
            //}

        });

        $("#btn_testCallback").on('click', function () {
            tblQuestions.ajax.reload(null, false); // user pagination is not reset on reload
        });
        /* DataTable section END */


        /**
         * Add option section
         */
        const addOptionToTable = () => {

        }

        /**
         * Add option section
        */
        //const createOption = () => {
        //    let option = {};
        //    let order = $('#tbl-options tbody tr').length;

        //    if (order >= 0 && order < 3) {
        //        option = {
        //            Order: order + 1,
        //            Marks: $('#question_Marks').val().trim(),
        //            Description: $('#option_Description').val().trim(),
        //            OptionType: $("input[name='OptionType']:checked").val()
        //        }
        //    }
        //    return option;
        //}

        let addOptionButtonClickCounter = 0;

        let addOptionButton = $("#js-btn-AddOption");


        // On add option click, build option table
        addOptionButton.on("click", function () {
            if (!qaEntryFormValidation()) {
                return false;
            }

            let html = "";
            let serialNo = addOptionButtonClickCounter + 1;
            let optionBody = $("#option_Description").val();
            let optionTypeVal = $("input[name='OptionType']:checked").val();

            //disable radio button
            $("input[name='OptionType']").iCheck("disable");

            //Building htmlTable:
            if (optionTypeVal === "Single Answer") {
                html = `"<tr>
                            <td>${serialNo}</td>
                            <td><input type = "radio" name="OptionRadioButton"/> ${optionBody}</td>
                            <td><a href="#" class="js-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a> </td>
                         </tr>`;
            }

            if (optionTypeVal === "Multiple Answer") {
                html = `"<tr>
                            <td>${serialNo}</td>
                            <td><input type = "radio" name="OptionRadioButton${serialNo}"/> ${optionBody}</td>
                            <td><a href="#" class="js-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a> </td>
                         </tr>`;
            }

            $("#js-tbl-options").append(html);
            addOptionButtonClickCounter++;

            if (addOptionButtonClickCounter === 4) {
                addOptionButton.prop("disabled", true);
            }
        });


        // Remove option from html table and change button, radio button behavior
        $(document).on("click", ".js-remove-option", function() {
            let closestRow = $(this).closest("tr");

            if (confirm("Are you sure you want to Remove?")) {
                closestRow.remove();
                addOptionButtonClickCounter--;
            }

            if (addOptionButtonClickCounter < 4) {
                addOptionButton.removeAttr("disabled");
            }

            if (addOptionButtonClickCounter === 0) {
                $("input[name='OptionType']").iCheck("enable");
            }
        });

        /*
         * Add option END
         */
    });
})(jQuery);

