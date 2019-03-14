(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["product-product-module"],{

/***/ "./src/app/pages/product/create/product.service.ts":
/*!*********************************************************!*\
  !*** ./src/app/pages/product/create/product.service.ts ***!
  \*********************************************************/
/*! exports provided: ProductService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ProductService", function() { return ProductService; });
/* harmony import */ var rxjs_Rx__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! rxjs/Rx */ "./node_modules/rxjs-compat/_esm5/Rx.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/http */ "./node_modules/@angular/http/fesm5/http.js");
/* harmony import */ var rxjs_Observable__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs/Observable */ "./node_modules/rxjs-compat/_esm5/Observable.js");
/* harmony import */ var _app_config__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../../app.config */ "./src/app/app.config.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var ProductService = /** @class */ (function () {
    function ProductService(_http) {
        this._http = _http;
        this.BaseUrl = _app_config__WEBPACK_IMPORTED_MODULE_4__["CONFIG"].baseURL + '/odata/Product';
    }
    ProductService.prototype.setExpand = function (expand) {
        this.BaseUrl + '?$expand=' + expand;
    };
    ProductService.prototype.getList = function () {
        return this._http.get(this.BaseUrl)
            .map(function (res) { return res.json(); })
            .catch(this.handleError);
    };
    ProductService.prototype.createProduct = function (product) {
        return this._http.post(this.BaseUrl, JSON.stringify(product), new _angular_http__WEBPACK_IMPORTED_MODULE_2__["RequestOptions"]({ headers: _app_config__WEBPACK_IMPORTED_MODULE_4__["CONFIG"].headers }));
    };
    ProductService.prototype.deleteProduct = function (Id) {
        console.log("Delete " + Id);
        return this._http.delete(this.BaseUrl + '(' + Id + ')');
    };
    ProductService.prototype.updateProduct = function (product) {
        return this._http.put(this.BaseUrl + '(' + product.Id + ')', JSON.stringify(product), new _angular_http__WEBPACK_IMPORTED_MODULE_2__["RequestOptions"]({ headers: _app_config__WEBPACK_IMPORTED_MODULE_4__["CONFIG"].headers }));
    };
    ProductService.prototype.handleError = function (error) {
        console.error(error);
        return rxjs_Observable__WEBPACK_IMPORTED_MODULE_3__["Observable"].throw(error.json().error || 'Server error');
    };
    ProductService = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Injectable"])(),
        __metadata("design:paramtypes", [_angular_http__WEBPACK_IMPORTED_MODULE_2__["Http"]])
    ], ProductService);
    return ProductService;
}());



/***/ })

}]);
//# sourceMappingURL=product-product-module.js.map