(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["app-pages-pages-module"],{

/***/ "./src/app/pages/dashboard/dashboard.component.html":
/*!**********************************************************!*\
  !*** ./src/app/pages/dashboard/dashboard.component.html ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n  <div class=\"col-md-6\">\r\n      <nb-card>\r\n          <nb-card-header>Báo cáo tồn kho </nb-card-header>\r\n          <nb-card-body>\r\n            <div class=\"form-group\">\r\n              <div class=\"row\">\r\n                <div class=\"col-md-6\">\r\n                    <select class=\"form-control\" (change)=\"selectFromWarehouseHandler($event)\">\r\n                      <option>--Từ kho--</option>\r\n                        <option *ngFor=\"let item of listWarehouse\" value= {{item.Id}}>\r\n                          {{item.Name}}\r\n                        </option>\r\n                    </select>\r\n                  </div>\r\n                <div class=\"col-md-6\">\r\n                    <input nbInput placeholder=\"Từ ngày ---- Đến ngày\" [nbDatepicker]=\"rangepicker\" class=\"form-control\">\r\n                    <nb-rangepicker (rangeChange)=\"pickerDate($event)\" format=\"MM.dd.yyyy\" #rangepicker  ></nb-rangepicker>\r\n                </div>\r\n              </div>\r\n            </div>\r\n            \r\n            <div class=\"form-group\">\r\n              <ng2-smart-table [settings]=\"settings\" [source]=\"list\" ></ng2-smart-table>  \r\n            </div>\r\n          </nb-card-body>\r\n        </nb-card>\r\n  </div>\r\n  <div class=\"col-md-6\">\r\n      <nb-card>\r\n          <nb-card-header>Báo cáo công nợ - ({{this.TongNo | number}})</nb-card-header>\r\n          <nb-card-body>\r\n            <div class=\"form-group\">\r\n              <div class=\"row\">\r\n                <div class=\"col-md-6\">\r\n                  <input nbInput placeholder=\"Từ ngày ---- Đến ngày\" [nbDatepicker]=\"rangepicker1\" class=\"form-control\">\r\n                  <nb-rangepicker (rangeChange)=\"pickerDate1($event)\" format=\"MM.dd.yyyy\" #rangepicker1  ></nb-rangepicker>\r\n                </div>\r\n              </div>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                \r\n            </div>\r\n            <div class=\"form-group\">\r\n              <ng2-smart-table [settings]=\"debtReport\" [source]=\"debt\"></ng2-smart-table>  \r\n            </div>\r\n          </nb-card-body>\r\n        </nb-card>\r\n    </div>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/pages/dashboard/dashboard.component.ts":
/*!********************************************************!*\
  !*** ./src/app/pages/dashboard/dashboard.component.ts ***!
  \********************************************************/
/*! exports provided: DashboardComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DashboardComponent", function() { return DashboardComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _nebular_theme__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @nebular/theme */ "./node_modules/@nebular/theme/index.js");
/* harmony import */ var rxjs_operators_takeWhile__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators/takeWhile */ "./node_modules/rxjs-compat/_esm5/operators/takeWhile.js");
/* harmony import */ var _dashboard_dashboard_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../dashboard/dashboard.service */ "./src/app/pages/dashboard/dashboard.service.ts");
/* harmony import */ var _warehouse_list_list_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../warehouse/list/list.service */ "./src/app/pages/warehouse/list/list.service.ts");
/* harmony import */ var _warehouse_base_search__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../warehouse/base/search */ "./src/app/pages/warehouse/base/search.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
var __assign = (undefined && undefined.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var DashboardComponent = /** @class */ (function () {
    function DashboardComponent(themeService, _dashboardService, _listService, _datePipe) {
        var _this = this;
        this.themeService = themeService;
        this._dashboardService = _dashboardService;
        this._listService = _listService;
        this._datePipe = _datePipe;
        this.start = "";
        this.end = "";
        this.startDebt = "";
        this.endDebt = "";
        this.TongNo = 0;
        this.searchModel = new _warehouse_base_search__WEBPACK_IMPORTED_MODULE_5__["Search"]();
        this.alive = true;
        this.lightCard = {
            title: 'Light',
            iconClass: 'nb-lightbulb',
            type: 'primary',
        };
        this.rollerShadesCard = {
            title: 'Roller Shades',
            iconClass: 'nb-roller-shades',
            type: 'success',
        };
        this.wirelessAudioCard = {
            title: 'Wireless Audio',
            iconClass: 'nb-audio',
            type: 'info',
        };
        this.coffeeMakerCard = {
            title: 'Coffee Maker',
            iconClass: 'nb-coffee-maker',
            type: 'warning',
        };
        this.commonStatusCardsSet = [
            this.lightCard,
            this.rollerShadesCard,
            this.wirelessAudioCard,
            this.coffeeMakerCard,
        ];
        this.statusCardsByThemes = {
            default: this.commonStatusCardsSet,
            cosmic: this.commonStatusCardsSet,
            corporate: [
                __assign({}, this.lightCard, { type: 'warning' }),
                __assign({}, this.rollerShadesCard, { type: 'primary' }),
                __assign({}, this.wirelessAudioCard, { type: 'danger' }),
                __assign({}, this.coffeeMakerCard, { type: 'secondary' }),
            ],
        };
        this.settings = {
            mode: 'external',
            hideSubHeader: true,
            actions: {
                add: false,
                delete: false,
                edit: false,
            },
            columns: {
                ProductName: {
                    title: 'Sản phẩm',
                    filter: false
                },
                OpeningStock: {
                    title: 'Tồn đầu',
                    filter: false
                },
                ImportQuantity: {
                    title: 'Nhập',
                    filter: false
                },
                ExportQuantity: {
                    title: 'Xuất',
                    filter: false
                },
                ClosingStock: {
                    title: 'Tồn cuối',
                    filter: false
                },
            }
        };
        this.debtReport = {
            mode: 'external',
            hideSubHeader: true,
            actions: {
                add: false,
                delete: false,
                edit: false,
            },
            columns: {
                WarehouseName: {
                    title: 'Đại lý',
                    filter: false
                },
                NoDauKy: {
                    title: 'Đầu kỳ',
                    filter: false,
                    valuePrepareFunction: function (value) { return value === 'Total' ? value : Intl.NumberFormat().format(value); }
                },
                NoCuoiKy: {
                    title: 'Cuối kỳ',
                    filter: false,
                    valuePrepareFunction: function (value) { return value === 'Total' ? value : Intl.NumberFormat().format(value); }
                },
            }
        };
        this.themeService.getJsTheme()
            .pipe(Object(rxjs_operators_takeWhile__WEBPACK_IMPORTED_MODULE_2__["takeWhile"])(function () { return _this.alive; }))
            .subscribe(function (theme) {
            _this.statusCards = _this.statusCardsByThemes[theme.name];
        });
    }
    DashboardComponent.prototype.pickerDate = function (event) {
        this.start = this._datePipe.transform(new Date(event.start), "yyyy-MM-dd");
        this.end = this._datePipe.transform(new Date(event.end), "yyyy-MM-dd");
        this.searchModel.from = this.start;
        this.searchModel.to = this.end;
        console.log(this.start);
        console.log(this.end);
        this.getListFilter();
        console.log(this.list);
    }; //'T'HH:mm:ss.SSS'Z'
    DashboardComponent.prototype.pickerDate1 = function (event) {
        this.startDebt = this._datePipe.transform(new Date(event.start), "yyyy-MM-dd");
        this.endDebt = this._datePipe.transform(new Date(event.end), "yyyy-MM-dd");
        this.searchModel.from = this.startDebt;
        this.searchModel.to = this.endDebt;
        console.log(this.startDebt);
        console.log(this.endDebt);
        this.getDebtFilter();
        this.getDebtTotal();
    };
    DashboardComponent.prototype.ngOnInit = function () {
        this.getListWarehouse();
        this.getDebtFilter();
        this.getDebtTotal();
    };
    DashboardComponent.prototype.getListStock = function () {
        var _this = this;
        this._dashboardService.getList()
            .subscribe(function (value) {
            if (value[0] != null) {
                _this.list = value[0].StockList;
            }
            else {
                _this.list = null;
            }
        }, function (error) {
            console.log(error);
        });
    };
    DashboardComponent.prototype.getListDebt = function () {
        var _this = this;
        this._dashboardService.getReport()
            .subscribe(function (value) {
            _this.debt = value.value;
            console.log(_this.debt);
        }, function (error) {
            console.log(error);
        });
    };
    DashboardComponent.prototype.getListWarehouse = function () {
        var _this = this;
        this._listService.getList()
            .subscribe(function (value) {
            _this.listWarehouse = value.value;
        }, function (error) {
            console.log(error);
        });
    };
    DashboardComponent.prototype.getListFilter = function () {
        var _this = this;
        this._dashboardService.getFilterList(this.searchModel)
            .subscribe(function (value) {
            console.log(value);
            if (value[0] != null) {
                _this.list = value[0].StockList;
            }
            else {
                _this.list = null;
            }
            //his.list = value[0].StockList;
        }, function (error) {
            console.log(error);
        });
    };
    DashboardComponent.prototype.getDebtFilter = function () {
        var _this = this;
        this._dashboardService.getReportFilter(this.searchModel)
            .subscribe(function (value) {
            console.log(value);
            _this.debt = value;
            console.log(_this.debt);
        }, function (error) {
            console.log(error);
        });
    };
    DashboardComponent.prototype.getDebtTotal = function () {
        var _this = this;
        this._dashboardService.getReportTotal(this.searchModel)
            .subscribe(function (value) {
            console.log(value);
            _this.TongNo = value;
            console.log(_this.TongNo);
        }, function (error) {
            console.log(error);
        });
    };
    DashboardComponent.prototype.selectFromWarehouseHandler = function (event) {
        this.searchModel.Id = event.target.value;
        this.getListFilter();
        console.log(this.list);
    };
    DashboardComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'ngx-dashboard',
            template: __webpack_require__(/*! ./dashboard.component.html */ "./src/app/pages/dashboard/dashboard.component.html"),
            providers: [_dashboard_dashboard_service__WEBPACK_IMPORTED_MODULE_3__["DashboardService"], _warehouse_list_list_service__WEBPACK_IMPORTED_MODULE_4__["ListService"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["DatePipe"]]
        }),
        __metadata("design:paramtypes", [_nebular_theme__WEBPACK_IMPORTED_MODULE_1__["NbThemeService"],
            _dashboard_dashboard_service__WEBPACK_IMPORTED_MODULE_3__["DashboardService"],
            _warehouse_list_list_service__WEBPACK_IMPORTED_MODULE_4__["ListService"],
            _angular_common__WEBPACK_IMPORTED_MODULE_6__["DatePipe"]])
    ], DashboardComponent);
    return DashboardComponent;
}());



/***/ }),

/***/ "./src/app/pages/dashboard/dashboard.module.ts":
/*!*****************************************************!*\
  !*** ./src/app/pages/dashboard/dashboard.module.ts ***!
  \*****************************************************/
/*! exports provided: DashboardModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DashboardModule", function() { return DashboardModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _theme_theme_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../@theme/theme.module */ "./src/app/@theme/theme.module.ts");
/* harmony import */ var _dashboard_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./dashboard.component */ "./src/app/pages/dashboard/dashboard.component.ts");
/* harmony import */ var _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @ng-bootstrap/ng-bootstrap */ "./node_modules/@ng-bootstrap/ng-bootstrap/fesm5/ng-bootstrap.js");
/* harmony import */ var _nebular_theme__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @nebular/theme */ "./node_modules/@nebular/theme/index.js");
/* harmony import */ var _nebular_moment__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @nebular/moment */ "./node_modules/@nebular/moment/index.js");
/* harmony import */ var ng2_smart_table__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ng2-smart-table */ "./node_modules/ng2-smart-table/index.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};







var DashboardModule = /** @class */ (function () {
    function DashboardModule() {
    }
    DashboardModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"])({
            imports: [
                _theme_theme_module__WEBPACK_IMPORTED_MODULE_1__["ThemeModule"],
                _ng_bootstrap_ng_bootstrap__WEBPACK_IMPORTED_MODULE_3__["NgbModule"].forRoot(),
                ng2_smart_table__WEBPACK_IMPORTED_MODULE_6__["Ng2SmartTableModule"],
                _nebular_theme__WEBPACK_IMPORTED_MODULE_4__["NbDatepickerModule"],
                _nebular_moment__WEBPACK_IMPORTED_MODULE_5__["NbMomentDateModule"]
            ],
            declarations: [
                _dashboard_component__WEBPACK_IMPORTED_MODULE_2__["DashboardComponent"],
            ]
        })
    ], DashboardModule);
    return DashboardModule;
}());



/***/ }),

/***/ "./src/app/pages/miscellaneous/miscellaneous-routing.module.ts":
/*!*********************************************************************!*\
  !*** ./src/app/pages/miscellaneous/miscellaneous-routing.module.ts ***!
  \*********************************************************************/
/*! exports provided: MiscellaneousRoutingModule, routedComponents */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MiscellaneousRoutingModule", function() { return MiscellaneousRoutingModule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "routedComponents", function() { return routedComponents; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _miscellaneous_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./miscellaneous.component */ "./src/app/pages/miscellaneous/miscellaneous.component.ts");
/* harmony import */ var _not_found_not_found_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./not-found/not-found.component */ "./src/app/pages/miscellaneous/not-found/not-found.component.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var routes = [{
        path: '',
        component: _miscellaneous_component__WEBPACK_IMPORTED_MODULE_2__["MiscellaneousComponent"],
        children: [{
                path: '404',
                component: _not_found_not_found_component__WEBPACK_IMPORTED_MODULE_3__["NotFoundComponent"],
            }],
    }];
var MiscellaneousRoutingModule = /** @class */ (function () {
    function MiscellaneousRoutingModule() {
    }
    MiscellaneousRoutingModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"])({
            imports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forChild(routes)],
            exports: [_angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]],
        })
    ], MiscellaneousRoutingModule);
    return MiscellaneousRoutingModule;
}());

var routedComponents = [
    _miscellaneous_component__WEBPACK_IMPORTED_MODULE_2__["MiscellaneousComponent"],
    _not_found_not_found_component__WEBPACK_IMPORTED_MODULE_3__["NotFoundComponent"],
];


/***/ }),

/***/ "./src/app/pages/miscellaneous/miscellaneous.component.ts":
/*!****************************************************************!*\
  !*** ./src/app/pages/miscellaneous/miscellaneous.component.ts ***!
  \****************************************************************/
/*! exports provided: MiscellaneousComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MiscellaneousComponent", function() { return MiscellaneousComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var MiscellaneousComponent = /** @class */ (function () {
    function MiscellaneousComponent() {
    }
    MiscellaneousComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'ngx-miscellaneous',
            template: "\n    <router-outlet></router-outlet>\n  ",
        })
    ], MiscellaneousComponent);
    return MiscellaneousComponent;
}());



/***/ }),

/***/ "./src/app/pages/miscellaneous/miscellaneous.module.ts":
/*!*************************************************************!*\
  !*** ./src/app/pages/miscellaneous/miscellaneous.module.ts ***!
  \*************************************************************/
/*! exports provided: MiscellaneousModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MiscellaneousModule", function() { return MiscellaneousModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _theme_theme_module__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../@theme/theme.module */ "./src/app/@theme/theme.module.ts");
/* harmony import */ var _miscellaneous_routing_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./miscellaneous-routing.module */ "./src/app/pages/miscellaneous/miscellaneous-routing.module.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var MiscellaneousModule = /** @class */ (function () {
    function MiscellaneousModule() {
    }
    MiscellaneousModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"])({
            imports: [
                _theme_theme_module__WEBPACK_IMPORTED_MODULE_1__["ThemeModule"],
                _miscellaneous_routing_module__WEBPACK_IMPORTED_MODULE_2__["MiscellaneousRoutingModule"],
            ],
            declarations: _miscellaneous_routing_module__WEBPACK_IMPORTED_MODULE_2__["routedComponents"].slice(),
        })
    ], MiscellaneousModule);
    return MiscellaneousModule;
}());



/***/ }),

/***/ "./src/app/pages/miscellaneous/not-found/not-found.component.html":
/*!************************************************************************!*\
  !*** ./src/app/pages/miscellaneous/not-found/not-found.component.html ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"row\">\r\n  <div class=\"col-md-12\">\r\n    <nb-card>\r\n      <nb-card-body>\r\n        <div class=\"flex-centered col-xl-4 col-lg-6 col-md-8 col-sm-12\">\r\n          <h2 class=\"title\">404 Page Not Found</h2>\r\n          <small class=\"sub-title\">The page you were looking for doesn't exist</small>\r\n          <button (click)=\"goToHome()\" type=\"button\" class=\"btn btn-block btn-hero-primary\">\r\n            Take me home\r\n          </button>\r\n        </div>\r\n      </nb-card-body>\r\n    </nb-card>\r\n  </div>\r\n</div>"

/***/ }),

/***/ "./src/app/pages/miscellaneous/not-found/not-found.component.scss":
/*!************************************************************************!*\
  !*** ./src/app/pages/miscellaneous/not-found/not-found.component.scss ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".flex-centered {\n  margin: auto; }\n\nnb-card-body {\n  display: -webkit-box;\n  display: -ms-flexbox;\n  display: flex; }\n\n.title {\n  text-align: center; }\n\n.sub-title {\n  text-align: center;\n  display: block;\n  margin-bottom: 3rem; }\n\n.btn {\n  margin-bottom: 2rem; }\n\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvcGFnZXMvbWlzY2VsbGFuZW91cy9ub3QtZm91bmQvUzpcXEtob1BpdGVjaEJhY2tcXGludmVudG9yeVxcSW52ZW50b3J5L3NyY1xcYXBwXFxwYWdlc1xcbWlzY2VsbGFuZW91c1xcbm90LWZvdW5kXFxub3QtZm91bmQuY29tcG9uZW50LnNjc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7RUFDRSxhQUFZLEVBQ2I7O0FBQ0Q7RUFDRSxxQkFBYTtFQUFiLHFCQUFhO0VBQWIsY0FBYSxFQUNkOztBQUVEO0VBQ0UsbUJBQWtCLEVBQ25COztBQUVEO0VBQ0UsbUJBQWtCO0VBQ2xCLGVBQWM7RUFDZCxvQkFBbUIsRUFDcEI7O0FBRUQ7RUFDRSxvQkFBbUIsRUFDcEIiLCJmaWxlIjoic3JjL2FwcC9wYWdlcy9taXNjZWxsYW5lb3VzL25vdC1mb3VuZC9ub3QtZm91bmQuY29tcG9uZW50LnNjc3MiLCJzb3VyY2VzQ29udGVudCI6WyIuZmxleC1jZW50ZXJlZCB7XHJcbiAgbWFyZ2luOiBhdXRvO1xyXG59XHJcbm5iLWNhcmQtYm9keSB7XHJcbiAgZGlzcGxheTogZmxleDtcclxufVxyXG5cclxuLnRpdGxlIHtcclxuICB0ZXh0LWFsaWduOiBjZW50ZXI7XHJcbn1cclxuXHJcbi5zdWItdGl0bGUge1xyXG4gIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICBkaXNwbGF5OiBibG9jaztcclxuICBtYXJnaW4tYm90dG9tOiAzcmVtO1xyXG59XHJcblxyXG4uYnRuIHtcclxuICBtYXJnaW4tYm90dG9tOiAycmVtO1xyXG59XHJcbiJdfQ== */"

/***/ }),

/***/ "./src/app/pages/miscellaneous/not-found/not-found.component.ts":
/*!**********************************************************************!*\
  !*** ./src/app/pages/miscellaneous/not-found/not-found.component.ts ***!
  \**********************************************************************/
/*! exports provided: NotFoundComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "NotFoundComponent", function() { return NotFoundComponent; });
/* harmony import */ var _nebular_theme__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @nebular/theme */ "./node_modules/@nebular/theme/index.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var NotFoundComponent = /** @class */ (function () {
    function NotFoundComponent(menuService) {
        this.menuService = menuService;
    }
    NotFoundComponent.prototype.goToHome = function () {
        this.menuService.navigateHome();
    };
    NotFoundComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'ngx-not-found',
            styles: [__webpack_require__(/*! ./not-found.component.scss */ "./src/app/pages/miscellaneous/not-found/not-found.component.scss")],
            template: __webpack_require__(/*! ./not-found.component.html */ "./src/app/pages/miscellaneous/not-found/not-found.component.html"),
        }),
        __metadata("design:paramtypes", [_nebular_theme__WEBPACK_IMPORTED_MODULE_0__["NbMenuService"]])
    ], NotFoundComponent);
    return NotFoundComponent;
}());



/***/ }),

/***/ "./src/app/pages/pages-menu.ts":
/*!*************************************!*\
  !*** ./src/app/pages/pages-menu.ts ***!
  \*************************************/
/*! exports provided: MENU_ITEMS */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MENU_ITEMS", function() { return MENU_ITEMS; });
var MENU_ITEMS = [
    {
        title: 'Thống kê',
        icon: 'nb-bar-chart',
        link: '/pages/dashboard',
        home: true,
    },
    {
        title: 'Quản kho',
        icon: 'nb-home',
        children: [
            {
                title: 'Kho',
                link: '/pages/warehouse/list',
            },
            {
                title: 'Hoạt động',
                link: '/pages/warehouse/log',
            },
            {
                title: 'Nhập kho',
                link: '/pages/warehouse/import',
            },
            {
                title: 'Xuất kho',
                link: '/pages/warehouse/export',
            },
            {
                title: 'Chuyển kho',
                link: '/pages/warehouse/transfer',
            }
        ]
    },
    {
        title: 'Công nợ',
        icon: 'nb-compose',
        children: [
            {
                title: 'DS phiếu ghi nợ',
                link: '/pages/debtnote/list',
            },
            {
                title: 'DS phiếu thu nợ',
                link: '/pages/debtnote/listdebtcollect',
            },
            {
                title: 'Tạo ghi công nợ',
                link: '/pages/debtnote/create',
            },
            {
                title: 'Tạo thu công nợ',
                link: '/pages/debtnote/createcollectdebtnote',
            }
        ]
    },
    {
        title: 'Sản phẩm',
        icon: 'nb-grid-a',
        children: [
            {
                title: 'Thêm sản phẩm',
                link: '/pages/product/create',
            }
        ]
    },
    {
        title: 'Đăng xuất',
        icon: 'nb-power',
        link: '/auth/login/'
    }
];


/***/ }),

/***/ "./src/app/pages/pages-routing.module.ts":
/*!***********************************************!*\
  !*** ./src/app/pages/pages-routing.module.ts ***!
  \***********************************************/
/*! exports provided: PagesRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PagesRoutingModule", function() { return PagesRoutingModule; });
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _pages_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./pages.component */ "./src/app/pages/pages.component.ts");
/* harmony import */ var _dashboard_dashboard_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./dashboard/dashboard.component */ "./src/app/pages/dashboard/dashboard.component.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var routes = [{
        path: '',
        component: _pages_component__WEBPACK_IMPORTED_MODULE_2__["PagesComponent"],
        children: [
            {
                path: 'dashboard',
                component: _dashboard_dashboard_component__WEBPACK_IMPORTED_MODULE_3__["DashboardComponent"],
            },
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full',
            },
            {
                path: 'product',
                loadChildren: './product/product.module#ProductModule',
            },
            {
                path: 'warehouse',
                loadChildren: './warehouse/warehouse.module#WarehouseModule',
            },
            {
                path: 'debtnote',
                loadChildren: './debtnote/debtnote.module#DebtNoteModule',
            }
        ],
    }];
var PagesRoutingModule = /** @class */ (function () {
    function PagesRoutingModule() {
    }
    PagesRoutingModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["NgModule"])({
            imports: [_angular_router__WEBPACK_IMPORTED_MODULE_0__["RouterModule"].forChild(routes)],
            exports: [_angular_router__WEBPACK_IMPORTED_MODULE_0__["RouterModule"]],
        })
    ], PagesRoutingModule);
    return PagesRoutingModule;
}());



/***/ }),

/***/ "./src/app/pages/pages.component.ts":
/*!******************************************!*\
  !*** ./src/app/pages/pages.component.ts ***!
  \******************************************/
/*! exports provided: PagesComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PagesComponent", function() { return PagesComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _pages_menu__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./pages-menu */ "./src/app/pages/pages-menu.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _angular_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/http */ "./node_modules/@angular/http/fesm5/http.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var PagesComponent = /** @class */ (function () {
    function PagesComponent(http, router) {
        this.http = http;
        this.router = router;
        this.menu = _pages_menu__WEBPACK_IMPORTED_MODULE_1__["MENU_ITEMS"];
        var logged = localStorage.getItem("loggedIn");
        console.log(logged);
        if (logged == null) {
            this.router.navigate(['/auth/login']);
        }
        else {
            this.router.navigate(['/page/dashboard']);
        }
    }
    PagesComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'ngx-pages',
            template: "\n    <ngx-sample-layout>\n      <nb-menu [items]=\"menu\"></nb-menu>\n      <router-outlet></router-outlet>\n    </ngx-sample-layout>\n  ",
        }),
        __metadata("design:paramtypes", [_angular_http__WEBPACK_IMPORTED_MODULE_3__["Http"], _angular_router__WEBPACK_IMPORTED_MODULE_2__["Router"]])
    ], PagesComponent);
    return PagesComponent;
}());



/***/ }),

/***/ "./src/app/pages/pages.module.ts":
/*!***************************************!*\
  !*** ./src/app/pages/pages.module.ts ***!
  \***************************************/
/*! exports provided: PagesModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PagesModule", function() { return PagesModule; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _pages_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./pages.component */ "./src/app/pages/pages.component.ts");
/* harmony import */ var _dashboard_dashboard_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./dashboard/dashboard.module */ "./src/app/pages/dashboard/dashboard.module.ts");
/* harmony import */ var _product_product_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./product/product.module */ "./src/app/pages/product/product.module.ts");
/* harmony import */ var _warehouse_warehouse_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./warehouse/warehouse.module */ "./src/app/pages/warehouse/warehouse.module.ts");
/* harmony import */ var _debtnote_debtnote_module__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./debtnote/debtnote.module */ "./src/app/pages/debtnote/debtnote.module.ts");
/* harmony import */ var _pages_routing_module__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./pages-routing.module */ "./src/app/pages/pages-routing.module.ts");
/* harmony import */ var _theme_theme_module__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../@theme/theme.module */ "./src/app/@theme/theme.module.ts");
/* harmony import */ var _miscellaneous_miscellaneous_module__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./miscellaneous/miscellaneous.module */ "./src/app/pages/miscellaneous/miscellaneous.module.ts");
/* harmony import */ var _nebular_theme__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @nebular/theme */ "./node_modules/@nebular/theme/index.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};










var PAGES_COMPONENTS = [
    _pages_component__WEBPACK_IMPORTED_MODULE_1__["PagesComponent"],
];
var PagesModule = /** @class */ (function () {
    function PagesModule() {
    }
    PagesModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"])({
            imports: [
                _pages_routing_module__WEBPACK_IMPORTED_MODULE_6__["PagesRoutingModule"],
                _theme_theme_module__WEBPACK_IMPORTED_MODULE_7__["ThemeModule"],
                _dashboard_dashboard_module__WEBPACK_IMPORTED_MODULE_2__["DashboardModule"],
                _miscellaneous_miscellaneous_module__WEBPACK_IMPORTED_MODULE_8__["MiscellaneousModule"],
                _product_product_module__WEBPACK_IMPORTED_MODULE_3__["ProductModule"],
                _warehouse_warehouse_module__WEBPACK_IMPORTED_MODULE_4__["WarehouseModule"],
                _debtnote_debtnote_module__WEBPACK_IMPORTED_MODULE_5__["DebtNoteModule"],
                _nebular_theme__WEBPACK_IMPORTED_MODULE_9__["NbDatepickerModule"]
            ],
            declarations: PAGES_COMPONENTS.slice(),
        })
    ], PagesModule);
    return PagesModule;
}());



/***/ })

}]);
//# sourceMappingURL=app-pages-pages-module.js.map