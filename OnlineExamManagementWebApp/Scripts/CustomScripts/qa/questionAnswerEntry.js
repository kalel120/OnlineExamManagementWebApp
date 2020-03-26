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

        const formQAEntry = $('#form_qaEntry');
        let btnQASubmit = $('#btn_qaSubmit');
        btnQASubmit.hide();
        /**END */

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
        const addOptionToTable = () => {

        }

        const createOption = () => {
            let option = {};
            let order = $('#tbl-options tbody tr').length;

            if (order >= 0 && order < 3) {
                option = {
                    Order: order + 1,
                    Description: $('#option_Description').val().trim(),
                    OptionType: $("input[name='OptionType']:checked").val()
                }
            }
            return option;
        }

        $('#btn_AddOption').on('click', function () {
            //if (!formQAEntry.parsley().validate()) {
            //    return;
            //}

            let option = createOption();
            if ($.isEmptyObject(option)) {
                alert('invalid');
                return;
            }
            console.log(createOption());

        });

        /* END */
        //end
    });
})(jQuery);

