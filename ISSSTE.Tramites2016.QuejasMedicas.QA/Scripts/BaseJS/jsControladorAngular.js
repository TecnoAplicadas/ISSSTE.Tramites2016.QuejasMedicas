function jsValidaAngular(url, modelo) {

    if (modelo != null) {
        modelo.url = url;
        var esModeloCorrecto = jsValidaModeloSinHtml(modelo);
        if (!esModeloCorrecto) return;
    }
}