var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var BlazorXamarin;
(function (BlazorXamarin) {
    var Blazor;
    (function (Blazor) {
        var TypeScript;
        (function (TypeScript) {
            class Localization {
                constructor() {
                    this._locale = navigator.languages &&
                        navigator.languages.length > 0 &&
                        navigator.languages[0];
                    if (!this._locale) {
                        this._locale = navigator["userLanguage"];
                    }
                    if (!this._locale) {
                        this._locale = navigator.language;
                    }
                    if (!this._locale) {
                        this._locale = navigator["browserLanguage"];
                    }
                    if (!this._locale) {
                        this._locale = "en";
                    }
                    this._ianaTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
                    this.getLocale = this.getLocale.bind(this);
                    this.getIanaTimeZone = this.getIanaTimeZone.bind(this);
                }
                getLocale() {
                    return this._locale;
                }
                getIanaTimeZone() {
                    return this._ianaTimeZone;
                }
            }
            TypeScript.Localization = Localization;
            function runAsyncFunction(callbackId, functionName, functionArgs) {
                const assemblyName = "BlazorXamarin.Blazor";
                let accumulator = window;
                for (let token of functionName.split(".")) {
                    accumulator = accumulator[token];
                }
                const otherFunction = accumulator;
                function makeFunctionCallAsync() {
                    return __awaiter(this, void 0, void 0, function* () {
                        try {
                            let value = yield otherFunction(...functionArgs);
                            if (value === undefined) {
                                value = null;
                            }
                            const result = JSON.stringify(value);
                            yield DotNet.invokeMethodAsync(assemblyName, "PromiseCallback", callbackId, result);
                        }
                        catch (e) {
                            if (!e) {
                                e = "Something went wrong";
                            }
                            const result = e.toString();
                            yield DotNet.invokeMethodAsync(assemblyName, "PromiseError", callbackId, result);
                        }
                    });
                }
                makeFunctionCallAsync();
                // Your function currently has to return something.
                return true;
            }
            TypeScript.runAsyncFunction = runAsyncFunction;
        })(TypeScript = Blazor.TypeScript || (Blazor.TypeScript = {}));
    })(Blazor = BlazorXamarin.Blazor || (BlazorXamarin.Blazor = {}));
})(BlazorXamarin || (BlazorXamarin = {}));
window["BlazorXamarin"] = {
    Blazor: {
        TypeScript: {
            Localization: new BlazorXamarin.Blazor.TypeScript.Localization(),
            runAsyncFunction: BlazorXamarin.Blazor.TypeScript.runAsyncFunction
        }
    }
};
const appLoading = document.getElementById("app-loading");
if (!!appLoading) {
    let locale = window["BlazorXamarin"].Blazor.TypeScript.Localization.getLocale();
    if (locale.indexOf("-") > -1) {
        locale = locale.split("-")[0];
    }
    switch (locale.toLowerCase()) {
        case "de":
            appLoading.innerText = "Wird geladen...";
            break;
        case "es":
            appLoading.innerText = "Cargando...";
            break;
        case "fr":
            appLoading.innerText = "Chargement...";
            break;
        case "en":
        default:
            appLoading.innerText = "Loading...";
            break;
    }
}
//# sourceMappingURL=site.js.map