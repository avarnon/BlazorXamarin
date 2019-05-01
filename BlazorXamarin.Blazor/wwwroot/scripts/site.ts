declare var DotNet: {
    invokeMethodAsync: (assemblyName: string, methodName: string, callbackId: string, result: string) => Promise<void>;
};

namespace BlazorXamarin.Blazor.TypeScript {
    export class Localization {
        private readonly _locale: string;
        private readonly _ianaTimeZone: string;

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

        public getLocale(): string {
            return this._locale;
        }

        public getIanaTimeZone(): string {
            return this._ianaTimeZone;
        }
    }

    export function runAsyncFunction(callbackId: string, functionName: string, functionArgs: Array<any>): boolean {
        const assemblyName: string = "BlazorXamarin.Blazor";
        let accumulator: any = window;

        for (let token of functionName.split(".")) {
            accumulator = accumulator[token];
        }

        const otherFunction: (...args: Array<any>) => Promise<any> = accumulator;

        async function makeFunctionCallAsync(): Promise<void> {
            try {
                let value: any = await otherFunction(...functionArgs);
                if (value === undefined) {
                    value = null;
                }

                const result: string = JSON.stringify(value);
                await DotNet.invokeMethodAsync(assemblyName, "PromiseCallback", callbackId, result);

            } catch (e) {
                if (!e) {
                    e = "Something went wrong";
                }

                const result: string = e.toString();

                await DotNet.invokeMethodAsync(assemblyName, "PromiseError", callbackId, result);
            }
        }

        makeFunctionCallAsync();

        // Your function currently has to return something.
        return true;
    }
}

window["BlazorXamarin"] = {
    Blazor: {
        TypeScript: {
            Localization: new BlazorXamarin.Blazor.TypeScript.Localization(),
            runAsyncFunction: BlazorXamarin.Blazor.TypeScript.runAsyncFunction
        }
    }
}


const appLoading: HTMLSpanElement = document.getElementById("app-loading");

if (!!appLoading) {
    let locale: string = window["BlazorXamarin"].Blazor.TypeScript.Localization.getLocale();

    if (locale.indexOf("-") > -1) {
        locale = locale.split("-")[0];
    }

    switch (locale.toLowerCase()) {
        case "fr":
            appLoading.innerText = "Chargement...";
            break;

        case "es":
            appLoading.innerText = "Cargando...";
            break;

        case "en":
        default:
            appLoading.innerText = "Loading...";
            break;
    }
}
