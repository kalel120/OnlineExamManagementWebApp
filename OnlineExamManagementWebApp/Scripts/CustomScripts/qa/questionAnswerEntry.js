// IIFE to wrap jQuery code

(($) => {
    $(() => {
        //start

        let loadingSpinner = $('#loading');
        loadingSpinner.hide();

        const examId = $('#js_examId').val();
        const middleRowPanel = $('#qa_entry_middleRow');
        const btnToggleQaSection = $('#btn-toggle-qaSection');
        
       const toggleDom = (domElement, status) => {
            if (status === "hide") {
                if (domElement.is(":visible")) {
                    domElement.hide();
                }
                return;
            }
            if (status === "show") {
                if (domElement.is(":hidden")) {
                    domElement.show();
                }
                return;
            }
        }

        toggleDom(middleRowPanel, "hide");

        $('#hide-section').on('click', function () {
            toggleDom(middleRowPanel, "hide");
        });

        btnToggleQaSection.on('click', function () {
            toggleDom(middleRowPanel, "show");
        });

        /**
         * DataTable section
         */
        const tblQuestions = $('#tbl-questions').DataTable({
            "ajax": {
                "beforeSend": () => { loadingSpinner.show(); },
                "url": `/QuestionAnswer/GetQuestionsByExamId?id=${examId}`,
                "contentType": "applicaiton/json",
                "data": function(d) {
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
                    "render": function (data,row) {
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


        $("#btn_testCallback").on('click', function() {
            tblQuestions.ajax.reload(null,false); // user pagination is not reset on reload
        });
        /* END */

        //end
    });
})(jQuery);

