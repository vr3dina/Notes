$(document).ready(function () {

    var BAKSPACE = 8;
    var SPACE = 32;
    var SELECTOR = "input[name=tags]";

    $(SELECTOR).keydown(function (e) {

        var inputs = $(SELECTOR);
        var parent = $(this).parent("div");

        // если нажали на SPACE и в данном инпуте уже есть значение
        // то запускаем процедуру клонирования
        if (e.keyCode === SPACE && $(this).val()) {

            $(this)
                .clone(true)        // клонируем
                .val(null)          // зануляем значение
                .appendTo(parent)   // добавляем на страницу
                .focus();           // ставим фокус

            return false;
        }

        // если нажали BAKSPACE и стирать больше нечего
        // и в списке инпутов еще есть что удалить
        if (e.keyCode === BAKSPACE && !$(this).val() && parent.children(SELECTOR).length > 1) {;
            // получаем индекс текущего
            var index = inputs.index($(this));

            // удаляем элемент
            $(this).remove();

            // ставим фокус на предыдущий или первый элемент
            inputs.eq(index === 0 ? 1 : index - 1).focus();

            return false;
        }
    });

});