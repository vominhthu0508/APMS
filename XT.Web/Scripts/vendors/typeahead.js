
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//Jquery Autocomplete

function monkeyPatchReplace(label, term)
{
    var new_label = '';
    for (var i = 0; i < label.length; i++)
    {
        var c = label[i];
        if (c.toLowerCase() == term.toLowerCase())
        {
            c = "<span class='highlight'>" + c + "</span>";
        }
        new_label += c;
    }

    return new_label;
}

function monkeyPatchAutocomplete() {

    // don't really need this, but in case I did, I could store it and chain
    var oldFn = $.ui.autocomplete.prototype._renderItem;

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        var term = this.term.toUpperCase();
        var label = item.label.toUpperCase();

        var re = new RegExp(term);
        var t = label.replace(re, "<span class='highlight'>" +
                term +
                "</span>");
        //var t = monkeyPatchReplace(item.label, this.term);
        return $("<li></li>")
            .data("item.autocomplete", item)
            .append("<a>" + t + "</a>")
            .appendTo(ul);
    };
}