function addListener(id) {
    var source = document.getElementById(id);
    source.addEventListener('input', onInput);
    source.addEventListener("focus", onFocus);
    source.addEventListener('focusout', onUnfocus);
    source.addEventListener('keydown', onKeydown);

}

function onInput(e) {
    var text = e.target.value;
    HybridWebView.SendInvokeMessageToDotNet("SetValue", [e.target.id, text]);
}

function onKeydown(e) {
    if (e.which == 13) {
        let active = document.activeElement;
        if (active == null) {
            return;
        }

        var next = findNextTabStop(active);
        if (next != null) {
            next.focus();
        }
        else {
            active.blur();
        }
    }
}

function findNextTabStop(el) {
    var universe = document.querySelectorAll('input, button, select, textarea, a[href]');
    var list = Array.prototype.filter.call(universe, function (item) { return item.tabIndex >= "0" });
    var index = list.indexOf(el);
    return list[index + 1] || null;
}

function removeFocus(id) {
    var el = document.getElementById(id);
    if (el == null) {
        return;
    }
    el.blur();
}

function onFocus(e) {
    HybridWebView.SendInvokeMessageToDotNet("SetFocused", [e.target.id, e.target.readOnly]);
}

function onUnfocus(e) {
    HybridWebView.SendInvokeMessageToDotNet("RemovedFocused", [e.target.id]);
}


function setText(name, text) {
    document.getElementById(name).value = text;
}

function getText(name) {
    return document.getElementById(name).value;
}