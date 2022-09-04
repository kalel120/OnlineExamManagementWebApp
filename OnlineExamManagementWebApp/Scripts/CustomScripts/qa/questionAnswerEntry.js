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
                    "Order": {
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
                    "Order": {
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
         *  END
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
        /* END */

        /**
         * Add option section
        */
        let addOptionButtonClickCounter = 0;
        let addOptionButton = $("#js-btn-AddOption");



        let isOptionBodyDuplicate = function (optionText) {
            let isDuplicated = false;
            $("#js-tbl-options tbody tr td:nth-child(2)")
                .each(function (index, element) {
                    if (optionText === $.trim($(element).text())) {
                        isDuplicated = true;
                    }
                });
            return isDuplicated;
        };


        // On add option click, build option table
        addOptionButton.on("click", function () {
            if (!qaEntryFormValidation()) {
                return false;
            }

            let html = "";
            let serialNo = addOptionButtonClickCounter + 1;
            let optionBody = $("#option_Description").val().trim();
            let optionTypeVal = $("input[name='OptionType']:checked").val();

            if (isOptionBodyDuplicate(optionBody)) {
                alert("Duplicate Option Text Detected");
                return false;
            }

            //disable radio button
            $("input[name='OptionType']").iCheck("disable");

            //Building htmlTable:
            if (optionTypeVal === "Single Answer") {
                html = `"<tr>
                            <td>${serialNo}</td>
                            <td><input type = "radio" name="OptionRadioButton"/> ${optionBody} </td>
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

            return true;
        });




        // Remove option from html table and change button, radio button behavior
        $(document).on("click",
            ".js-remove-option",
            function () {
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
         *  END
         */


        /**
         * Submit Q&A button functionality
         */

        const getOptionsTableData = function(listOfOptions) {
            $("#js-tbl-options tbody tr")
                .each(function (index, element) {
                    let option = {
                        "SerialNo": $(element).find("td:nth-child(1)").text(),
                        "OptionText": $.trim($(element).find("td:nth-child(2)").text()),
                        "IsCorrectAns": $(element).find("td input:radio").is(":checked")
                    }
                    listOfOptions.push(option);
                });
            return listOfOptions;
        };

        const getQaDataToSubmit = function(listOfOptions) {
            let questionAnsData = {
                Order: $("#question_Order").val(),
                Marks: $("#question_Marks").val(),
                QuestionDescription: $("#question_Description").val(),
                OptionType: $("input[name='OptionType']:checked").val(),
                Options: listOfOptions
            };
            return questionAnsData;
        };

        const isAnswerSelected = function (listOfOptions) {
            let isSelected = false;

            $.each(listOfOptions, function (key,value) {
                $.each(value, function(innerKey, innerValue) {
                    if (innerKey === "IsCorrectAns") {
                        if (innerValue) {
                            isSelected = true;
                        }
                    }
                    
                });
            });

            return isSelected;
        };

        $(document).on("click",
            '#js-btn-qaSubmit',
            function () {
                // loop through option table and get values
                let listOfOptions = getOptionsTableData(new Array());

                
                if (listOfOptions.length < 4) {
                    alert("Can't submit unless there are 4 options");
                    return false;
                }
                if (!isAnswerSelected(listOfOptions)) {
                    alert("Can't submit unless correct answer/answers are selected");
                    return false;
                }

                // Create questionAnswer data to submit
                let qaDataToSubmit = getQaDataToSubmit(listOfOptions);

                console.log(qaDataToSubmit);
            });

        /*
        * END
        */
    });
})(jQuery);

