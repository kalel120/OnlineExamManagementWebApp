
// IIFE to wrap jQuery code

(($) => {
    $(() => {
        //start
        const middleRowPanel = $('#qa_entry_middleRow');
        const btnToggleQaSection = $('#btn-toggle-qaSection');
        const tblQuestions = $('#tbl-questions');

        const toggleDom = (domElement, status) => {
            if (status === "hide") {
                if (domElement.is(":visible")) {
                    domElement.hide();
                }
                return;
            }
            if (status ==="show") {
                if (domElement.is(":hidden")) {
                    domElement.show();
                }
                return;
            }
        }

        toggleDom(middleRowPanel, "hide");

        $('#hide-section').on('click', function() {
            toggleDom(middleRowPanel,"hide");
        });

        btnToggleQaSection.on('click', function() {
            toggleDom(middleRowPanel,"show");
        });


        tblQuestions.DataTable();
        //end
    });
})(jQuery);

