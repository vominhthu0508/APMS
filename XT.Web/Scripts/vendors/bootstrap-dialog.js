
(function (a, b) {
    if (typeof module !== "undefined" && module.exports) {
        module.exports = b(require("jquery")(a))
    } else {
        if (typeof define === "function" && define.amd) {
            define("bootstrap-dialog", ["jquery"], function (c) {
                return b(c)
            })
        } else {
            a.BootstrapDialog = b(a.jQuery)
        }
    }
}(this, function (d) {
    var b = d.fn.modal.Constructor;
    var c = function (f, e) {
        b.call(this, f, e)
    };
    c.getModalVersion = function () {
        var e = null;
        if (typeof d.fn.modal.Constructor.VERSION === "undefined") {
            e = "v3.1"
        } else {
            if (/3\.2\.\d+/.test(d.fn.modal.Constructor.VERSION)) {
                e = "v3.2"
            } else {
                if (/3\.3\.[1,2]/.test(d.fn.modal.Constructor.VERSION)) {
                    e = "v3.3"
                } else {
                    e = "v3.3.4"
                }
            }
        }
        return e
    };
    c.ORIGINAL_BODY_PADDING = d("body").css("padding-right") || 0;
    c.METHODS_TO_OVERRIDE = {};
    c.METHODS_TO_OVERRIDE["v3.1"] = {};
    c.METHODS_TO_OVERRIDE["v3.2"] = {
        hide: function (g) {
            if (g) {
                g.preventDefault()
            }
            g = d.Event("hide.bs.modal");
            this.$element.trigger(g);
            if (!this.isShown || g.isDefaultPrevented()) {
                return
            }
            this.isShown = false;
            var f = this.getGlobalOpenedDialogs();
            if (f.length === 0) {
                this.$body.removeClass("modal-open")
            }
            this.resetScrollbar();
            this.escape();
            d(document).off("focusin.bs.modal");
            this.$element.removeClass("in").attr("aria-hidden", true).off("click.dismiss.bs.modal");
            d.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", d.proxy(this.hideModal, this)).emulateTransitionEnd(300) : this.hideModal()
        }
    };
    c.METHODS_TO_OVERRIDE["v3.3"] = {
        setScrollbar: function () {
            var e = c.ORIGINAL_BODY_PADDING;
            if (this.bodyIsOverflowing) {
                this.$body.css("padding-right", e + this.scrollbarWidth)
            }
        },
        resetScrollbar: function () {
            var e = this.getGlobalOpenedDialogs();
            if (e.length === 0) {
                this.$body.css("padding-right", c.ORIGINAL_BODY_PADDING)
            }
        },
        hideModal: function () {
            this.$element.hide();
            this.backdrop(d.proxy(function () {
                var e = this.getGlobalOpenedDialogs();
                if (e.length === 0) {
                    this.$body.removeClass("modal-open")
                }
                this.resetAdjustments();
                this.resetScrollbar();
                this.$element.trigger("hidden.bs.modal")
            }, this))
        }
    };
    c.METHODS_TO_OVERRIDE["v3.3.4"] = d.extend({}, c.METHODS_TO_OVERRIDE["v3.3"]);
    c.prototype = {
        constructor: c,
        getGlobalOpenedDialogs: function () {
            var e = [];
            d.each(a.dialogs, function (g, f) {
                if (f.isRealized() && f.isOpened()) {
                    e.push(f)
                }
            });
            return e
        }
    };
    c.prototype = d.extend(c.prototype, b.prototype, c.METHODS_TO_OVERRIDE[c.getModalVersion()]);
    var a = function (e) {
        this.defaultOptions = d.extend(true, {
            id: a.newGuid(),
            buttons: [],
            data: {},
            onshow: null,
            onshown: null,
            onhide: null,
            onhidden: null
        }, a.defaultOptions);
        this.indexedButtons = {};
        this.registeredButtonHotkeys = {};
        this.draggableData = {
            isMouseDown: false,
            mouseOffset: {}
        };
        this.realized = false;
        this.opened = false;
        this.initOptions(e);
        this.holdThisInstance()
    };
    a.BootstrapDialogModal = c;
    a.NAMESPACE = "bootstrap-dialog";
    a.TYPE_DEFAULT = "type-default";
    a.TYPE_INFO = "type-info";
    a.TYPE_PRIMARY = "type-primary";
    a.TYPE_SUCCESS = "type-success";
    a.TYPE_WARNING = "type-warning";
    a.TYPE_DANGER = "type-danger";
    a.DEFAULT_TEXTS = {};
    a.DEFAULT_TEXTS[a.TYPE_DEFAULT] = "Information";
    a.DEFAULT_TEXTS[a.TYPE_INFO] = "Information";
    a.DEFAULT_TEXTS[a.TYPE_PRIMARY] = "Information";
    a.DEFAULT_TEXTS[a.TYPE_SUCCESS] = "Success";
    a.DEFAULT_TEXTS[a.TYPE_WARNING] = "Warning";
    a.DEFAULT_TEXTS[a.TYPE_DANGER] = "Danger";
    a.DEFAULT_TEXTS.OK = "OK";
    a.DEFAULT_TEXTS.CANCEL = "Cancel";
    a.DEFAULT_TEXTS.CONFIRM = "Confirmation";
    a.SIZE_NORMAL = "size-normal";
    a.SIZE_SMALL = "size-small";
    a.SIZE_WIDE = "size-wide";
    a.SIZE_LARGE = "size-large";
    a.BUTTON_SIZES = {};
    a.BUTTON_SIZES[a.SIZE_NORMAL] = "";
    a.BUTTON_SIZES[a.SIZE_SMALL] = "";
    a.BUTTON_SIZES[a.SIZE_WIDE] = "";
    a.BUTTON_SIZES[a.SIZE_LARGE] = "btn-lg";
    a.ICON_SPINNER = "glyphicon glyphicon-asterisk";
    a.defaultOptions = {
        type: a.TYPE_PRIMARY,
        size: a.SIZE_NORMAL,
        cssClass: "",
        title: null,
        message: null,
        nl2br: true,
        closable: true,
        closeByBackdrop: true,
        closeByKeyboard: true,
        spinicon: a.ICON_SPINNER,
        autodestroy: true,
        draggable: false,
        animate: true,
        description: ""
    };
    a.configDefaultOptions = function (e) {
        a.defaultOptions = d.extend(true, a.defaultOptions, e)
    };
    a.dialogs = {};
    a.openAll = function () {
        d.each(a.dialogs, function (f, e) {
            e.open()
        })
    };
    a.closeAll = function () {
        d.each(a.dialogs, function (f, e) {
            e.close()
        })
    };
    a.moveFocus = function () {
        var e = null;
        d.each(a.dialogs, function (g, f) {
            e = f
        });
        if (e !== null && e.isRealized()) {
            e.getModal().focus()
        }
    };
    a.METHODS_TO_OVERRIDE = {};
    a.METHODS_TO_OVERRIDE["v3.1"] = {
        handleModalBackdropEvent: function () {
            this.getModal().on("click", {
                dialog: this
            }, function (e) {
                e.target === this && e.data.dialog.isClosable() && e.data.dialog.canCloseByBackdrop() && e.data.dialog.close()
            });
            return this
        },
        updateZIndex: function () {
            var g = 1040;
            var h = 1050;
            var i = 0;
            d.each(a.dialogs, function (j, k) {
                i++
            });
            var f = this.getModal();
            var e = f.data("bs.modal").$backdrop;
            f.css("z-index", h + (i - 1) * 20);
            e.css("z-index", g + (i - 1) * 20);
            return this
        },
        open: function () {
            !this.isRealized() && this.realize();
            this.getModal().modal("show");
            this.updateZIndex();
            return this
        }
    };
    a.METHODS_TO_OVERRIDE["v3.2"] = {
        handleModalBackdropEvent: a.METHODS_TO_OVERRIDE["v3.1"]["handleModalBackdropEvent"],
        updateZIndex: a.METHODS_TO_OVERRIDE["v3.1"]["updateZIndex"],
        open: a.METHODS_TO_OVERRIDE["v3.1"]["open"]
    };
    a.METHODS_TO_OVERRIDE["v3.3"] = {};
    a.METHODS_TO_OVERRIDE["v3.3.4"] = d.extend({}, a.METHODS_TO_OVERRIDE["v3.1"]);
    a.prototype = {
        constructor: a,
        initOptions: function (e) {
            this.options = d.extend(true, this.defaultOptions, e);
            return this
        },
        holdThisInstance: function () {
            a.dialogs[this.getId()] = this;
            return this
        },
        initModalStuff: function () {
            this.setModal(this.createModal()).setModalDialog(this.createModalDialog()).setModalContent(this.createModalContent()).setModalHeader(this.createModalHeader()).setModalBody(this.createModalBody()).setModalFooter(this.createModalFooter());
            this.getModal().append(this.getModalDialog());
            this.getModalDialog().append(this.getModalContent());
            this.getModalContent().append(this.getModalHeader()).append(this.getModalBody()).append(this.getModalFooter());
            return this
        },
        createModal: function () {
            var e = d('<div class="modal" tabindex="-1" role="dialog" aria-hidden="true"></div>');
            e.prop("id", this.getId()).attr("aria-labelledby", this.getId() + "_title");
            return e
        },
        getModal: function () {
            return this.$modal
        },
        setModal: function (e) {
            this.$modal = e;
            return this
        },
        createModalDialog: function () {
            return d('<div class="modal-dialog"></div>')
        },
        getModalDialog: function () {
            return this.$modalDialog
        },
        setModalDialog: function (e) {
            this.$modalDialog = e;
            return this
        },
        createModalContent: function () {
            return d('<div class="modal-content"></div>')
        },
        getModalContent: function () {
            return this.$modalContent
        },
        setModalContent: function (e) {
            this.$modalContent = e;
            return this
        },
        createModalHeader: function () {
            return d('<div class="modal-header"></div>')
        },
        getModalHeader: function () {
            return this.$modalHeader
        },
        setModalHeader: function (e) {
            this.$modalHeader = e;
            return this
        },
        createModalBody: function () {
            return d('<div class="modal-body"></div>')
        },
        getModalBody: function () {
            return this.$modalBody
        },
        setModalBody: function (e) {
            this.$modalBody = e;
            return this
        },
        createModalFooter: function () {
            return d('<div class="modal-footer"></div>')
        },
        getModalFooter: function () {
            return this.$modalFooter
        },
        setModalFooter: function (e) {
            this.$modalFooter = e;
            return this
        },
        createDynamicContent: function (f) {
            var e = null;
            if (typeof f === "function") {
                e = f.call(f, this)
            } else {
                e = f
            }
            if (typeof e === "string") {
                e = this.formatStringContent(e)
            }
            return e
        },
        formatStringContent: function (e) {
            if (this.options.nl2br) {
                return e.replace(/\r\n/g, "<br />").replace(/[\r\n]/g, "<br />")
            }
            return e
        },
        setData: function (e, f) {
            this.options.data[e] = f;
            return this
        },
        getData: function (e) {
            return this.options.data[e]
        },
        setId: function (e) {
            this.options.id = e;
            return this
        },
        getId: function () {
            return this.options.id
        },
        getType: function () {
            return this.options.type
        },
        setType: function (e) {
            this.options.type = e;
            this.updateType();
            return this
        },
        updateType: function () {
            if (this.isRealized()) {
                var e = [a.TYPE_DEFAULT, a.TYPE_INFO, a.TYPE_PRIMARY, a.TYPE_SUCCESS, a.TYPE_WARNING, a.TYPE_DANGER];
                this.getModal().removeClass(e.join(" ")).addClass(this.getType())
            }
            return this
        },
        getSize: function () {
            return this.options.size
        },
        setSize: function (e) {
            this.options.size = e;
            this.updateSize();
            return this
        },
        updateSize: function () {
            if (this.isRealized()) {
                var e = this;
                this.getModal().removeClass(a.SIZE_NORMAL).removeClass(a.SIZE_SMALL).removeClass(a.SIZE_WIDE).removeClass(a.SIZE_LARGE);
                this.getModal().addClass(this.getSize());
                this.getModalDialog().removeClass("modal-sm");
                if (this.getSize() === a.SIZE_SMALL) {
                    this.getModalDialog().addClass("modal-sm")
                }
                this.getModalDialog().removeClass("modal-lg");
                if (this.getSize() === a.SIZE_WIDE) {
                    this.getModalDialog().addClass("modal-lg")
                }
                d.each(this.options.buttons, function (g, i) {
                    var k = e.getButton(i.id);
                    var f = ["btn-lg", "btn-sm", "btn-xs"];
                    var j = false;
                    if (typeof i.cssClass === "string") {
                        var h = i.cssClass.split(" ");
                        d.each(h, function (l, m) {
                            if (d.inArray(m, f) !== -1) {
                                j = true
                            }
                        })
                    }
                    if (!j) {
                        k.removeClass(f.join(" "));
                        k.addClass(e.getButtonSize())
                    }
                })
            }
            return this
        },
        getCssClass: function () {
            return this.options.cssClass
        },
        setCssClass: function (e) {
            this.options.cssClass = e;
            return this
        },
        getTitle: function () {
            return this.options.title
        },
        setTitle: function (e) {
            this.options.title = e;
            this.updateTitle();
            return this
        },
        updateTitle: function () {
            if (this.isRealized()) {
                var e = this.getTitle() !== null ? this.createDynamicContent(this.getTitle()) : this.getDefaultText();
                this.getModalHeader().find("." + this.getNamespace("title")).html("").append(e).prop("id", this.getId() + "_title")
            }
            return this
        },
        getMessage: function () {
            return this.options.message
        },
        setMessage: function (e) {
            this.options.message = e;
            this.updateMessage();
            return this
        },
        updateMessage: function () {
            if (this.isRealized()) {
                var e = this.createDynamicContent(this.getMessage());
                this.getModalBody().find("." + this.getNamespace("message")).html("").append(e)
            }
            return this
        },
        isClosable: function () {
            return this.options.closable
        },
        setClosable: function (e) {
            this.options.closable = e;
            this.updateClosable();
            return this
        },
        setCloseByBackdrop: function (e) {
            this.options.closeByBackdrop = e;
            return this
        },
        canCloseByBackdrop: function () {
            return this.options.closeByBackdrop
        },
        setCloseByKeyboard: function (e) {
            this.options.closeByKeyboard = e;
            return this
        },
        canCloseByKeyboard: function () {
            return this.options.closeByKeyboard
        },
        isAnimate: function () {
            return this.options.animate
        },
        setAnimate: function (e) {
            this.options.animate = e;
            return this
        },
        updateAnimate: function () {
            if (this.isRealized()) {
                this.getModal().toggleClass("fade", this.isAnimate())
            }
            return this
        },
        getSpinicon: function () {
            return this.options.spinicon
        },
        setSpinicon: function (e) {
            this.options.spinicon = e;
            return this
        },
        addButton: function (e) {
            this.options.buttons.push(e);
            return this
        },
        addButtons: function (f) {
            var e = this;
            d.each(f, function (g, h) {
                e.addButton(h)
            });
            return this
        },
        getButtons: function () {
            return this.options.buttons
        },
        setButtons: function (e) {
            this.options.buttons = e;
            this.updateButtons();
            return this
        },
        getButton: function (e) {
            if (typeof this.indexedButtons[e] !== "undefined") {
                return this.indexedButtons[e]
            }
            return null
        },
        getButtonSize: function () {
            if (typeof a.BUTTON_SIZES[this.getSize()] !== "undefined") {
                return a.BUTTON_SIZES[this.getSize()]
            }
            return ""
        },
        updateButtons: function () {
            if (this.isRealized()) {
                if (this.getButtons().length === 0) {
                    this.getModalFooter().hide()
                } else {
                    this.getModalFooter().show().find("." + this.getNamespace("footer")).html("").append(this.createFooterButtons())
                }
            }
            return this
        },
        isAutodestroy: function () {
            return this.options.autodestroy
        },
        setAutodestroy: function (e) {
            this.options.autodestroy = e
        },
        getDescription: function () {
            return this.options.description
        },
        setDescription: function (e) {
            this.options.description = e;
            return this
        },
        getDefaultText: function () {
            return a.DEFAULT_TEXTS[this.getType()]
        },
        getNamespace: function (e) {
            return a.NAMESPACE + "-" + e
        },
        createHeaderContent: function () {
            var e = d("<div></div>");
            e.addClass(this.getNamespace("header"));
            e.append(this.createTitleContent());
            e.prepend(this.createCloseButton());
            return e
        },
        createTitleContent: function () {
            var e = d("<div></div>");
            e.addClass(this.getNamespace("title"));
            return e
        },
        createCloseButton: function () {
            var f = d("<div></div>");
            f.addClass(this.getNamespace("close-button"));
            var e = d('<button class="close">&times;</button>');
            f.append(e);
            f.on("click", {
                dialog: this
            }, function (g) {
                g.data.dialog.close()
            });
            return f
        },
        createBodyContent: function () {
            var e = d("<div></div>");
            e.addClass(this.getNamespace("body"));
            e.append(this.createMessageContent());
            return e
        },
        createMessageContent: function () {
            var e = d("<div></div>");
            e.addClass(this.getNamespace("message"));
            return e
        },
        createFooterContent: function () {
            var e = d("<div></div>");
            e.addClass(this.getNamespace("footer"));
            return e
        },
        createFooterButtons: function () {
            var e = this;
            var f = d("<div></div>");
            f.addClass(this.getNamespace("footer-buttons"));
            this.indexedButtons = {};
            d.each(this.options.buttons, function (g, h) {
                if (!h.id) {
                    h.id = a.newGuid()
                }
                var i = e.createButton(h);
                e.indexedButtons[h.id] = i;
                f.append(i)
            });
            return f
        },
        createButton: function (e) {
            var f = d('<button class="btn"></button>');
            f.prop("id", e.id);
            f.data("button", e);
            if (typeof e.icon !== "undefined" && d.trim(e.icon) !== "") {
                f.append(this.createButtonIcon(e.icon))
            }
            if (typeof e.label !== "undefined") {
                f.append(e.label)
            }
            if (typeof e.cssClass !== "undefined" && d.trim(e.cssClass) !== "") {
                f.addClass(e.cssClass)
            } else {
                f.addClass("btn-default")
            }
            if (typeof e.hotkey !== "undefined") {
                this.registeredButtonHotkeys[e.hotkey] = f
            }
            f.on("click", {
                dialog: this,
                $button: f,
                button: e
            }, function (i) {
                var h = i.data.dialog;
                var j = i.data.$button;
                var g = j.data("button");
                if (typeof g.action === "function") {
                    g.action.call(j, h, i)
                }
                if (g.autospin) {
                    j.toggleSpin(true)
                }
            });
            this.enhanceButton(f);
            return f
        },
        enhanceButton: function (e) {
            e.dialog = this;
            e.toggleEnable = function (f) {
                var g = this;
                if (typeof f !== "undefined") {
                    g.prop("disabled", !f).toggleClass("disabled", !f)
                } else {
                    g.prop("disabled", !g.prop("disabled"))
                }
                return g
            };
            e.enable = function () {
                var f = this;
                f.toggleEnable(true);
                return f
            };
            e.disable = function () {
                var f = this;
                f.toggleEnable(false);
                return f
            };
            e.toggleSpin = function (i) {
                var h = this;
                var g = h.dialog;
                var f = h.find("." + g.getNamespace("button-icon"));
                if (typeof i === "undefined") {
                    i = !(e.find(".icon-spin").length > 0)
                }
                if (i) {
                    f.hide();
                    e.prepend(g.createButtonIcon(g.getSpinicon()).addClass("icon-spin"))
                } else {
                    f.show();
                    e.find(".icon-spin").remove()
                }
                return h
            };
            e.spin = function () {
                var f = this;
                f.toggleSpin(true);
                return f
            };
            e.stopSpin = function () {
                var f = this;
                f.toggleSpin(false);
                return f
            };
            return this
        },
        createButtonIcon: function (f) {
            var e = d("<span></span>");
            e.addClass(this.getNamespace("button-icon")).addClass(f);
            return e
        },
        enableButtons: function (e) {
            d.each(this.indexedButtons, function (g, f) {
                f.toggleEnable(e)
            });
            return this
        },
        updateClosable: function () {
            if (this.isRealized()) {
                this.getModalHeader().find("." + this.getNamespace("close-button")).toggle(this.isClosable())
            }
            return this
        },
        onShow: function (e) {
            this.options.onshow = e;
            return this
        },
        onShown: function (e) {
            this.options.onshown = e;
            return this
        },
        onHide: function (e) {
            this.options.onhide = e;
            return this
        },
        onHidden: function (e) {
            this.options.onhidden = e;
            return this
        },
        isRealized: function () {
            return this.realized
        },
        setRealized: function (e) {
            this.realized = e;
            return this
        },
        isOpened: function () {
            return this.opened
        },
        setOpened: function (e) {
            this.opened = e;
            return this
        },
        handleModalEvents: function () {
            this.getModal().on("show.bs.modal", {
                dialog: this
            }, function (g) {
                var f = g.data.dialog;
                f.setOpened(true);
                if (f.isModalEvent(g) && typeof f.options.onshow === "function") {
                    var e = f.options.onshow(f);
                    if (e === false) {
                        f.setOpened(false)
                    }
                    return e
                }
            });
            this.getModal().on("shown.bs.modal", {
                dialog: this
            }, function (f) {
                var e = f.data.dialog;
                e.isModalEvent(f) && typeof e.options.onshown === "function" && e.options.onshown(e)
            });
            this.getModal().on("hide.bs.modal", {
                dialog: this
            }, function (f) {
                var e = f.data.dialog;
                e.setOpened(false);
                if (e.isModalEvent(f) && typeof e.options.onhide === "function") {
                    var g = e.options.onhide(e);
                    if (g === false) {
                        e.setOpened(true)
                    }
                    return g
                }
            });
            this.getModal().on("hidden.bs.modal", {
                dialog: this
            }, function (f) {
                var e = f.data.dialog;
                e.isModalEvent(f) && typeof e.options.onhidden === "function" && e.options.onhidden(e);
                if (e.isAutodestroy()) {
                    delete a.dialogs[e.getId()];
                    d(this).remove()
                }
                a.moveFocus()
            });
            this.handleModalBackdropEvent();
            this.getModal().on("keyup", {
                dialog: this
            }, function (e) {
                e.which === 27 && e.data.dialog.isClosable() && e.data.dialog.canCloseByKeyboard() && e.data.dialog.close()
            });
            this.getModal().on("keyup", {
                dialog: this
            }, function (f) {
                var e = f.data.dialog;
                if (typeof e.registeredButtonHotkeys[f.which] !== "undefined") {
                    var g = d(e.registeredButtonHotkeys[f.which]);
                    !g.prop("disabled") && g.focus().trigger("click")
                }
            });
            return this
        },
        handleModalBackdropEvent: function () {
            this.getModal().on("click", {
                dialog: this
            }, function (e) {
                d(e.target).hasClass("modal-backdrop") && e.data.dialog.isClosable() && e.data.dialog.canCloseByBackdrop() && e.data.dialog.close()
            });
            return this
        },
        isModalEvent: function (e) {
            return typeof e.namespace !== "undefined" && e.namespace === "bs.modal"
        },
        makeModalDraggable: function () {
            if (this.options.draggable) {
                this.getModalHeader().addClass(this.getNamespace("draggable")).on("mousedown", {
                    dialog: this
                }, function (g) {
                    var f = g.data.dialog;
                    f.draggableData.isMouseDown = true;
                    var e = f.getModalDialog().offset();
                    f.draggableData.mouseOffset = {
                        top: g.clientY - e.top,
                        left: g.clientX - e.left
                    }
                });
                this.getModal().on("mouseup mouseleave", {
                    dialog: this
                }, function (e) {
                    e.data.dialog.draggableData.isMouseDown = false
                });
                d("body").on("mousemove", {
                    dialog: this
                }, function (f) {
                    var e = f.data.dialog;
                    if (!e.draggableData.isMouseDown) {
                        return
                    }
                    e.getModalDialog().offset({
                        top: f.clientY - e.draggableData.mouseOffset.top,
                        left: f.clientX - e.draggableData.mouseOffset.left
                    })
                })
            }
            return this
        },
        realize: function () {
            this.initModalStuff();
            this.getModal().addClass(a.NAMESPACE).addClass(this.getCssClass());
            this.updateSize();
            if (this.getDescription()) {
                this.getModal().attr("aria-describedby", this.getDescription())
            }
            this.getModalFooter().append(this.createFooterContent());
            this.getModalHeader().append(this.createHeaderContent());
            this.getModalBody().append(this.createBodyContent());
            this.getModal().data("bs.modal", new c(this.getModal(), {
                backdrop: "static",
                keyboard: false,
                show: false
            }));
            this.makeModalDraggable();
            this.handleModalEvents();
            this.setRealized(true);
            this.updateButtons();
            this.updateType();
            this.updateTitle();
            this.updateMessage();
            this.updateClosable();
            this.updateAnimate();
            this.updateSize();
            return this
        },
        open: function () {
            !this.isRealized() && this.realize();
            this.getModal().modal("show");
            return this
        },
        close: function () {
            this.getModal().modal("hide");
            return this
        }
    };
    a.prototype = d.extend(a.prototype, a.METHODS_TO_OVERRIDE[c.getModalVersion()]);
    a.newGuid = function () {
        return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (g) {
            var f = Math.random() * 16 | 0,
                e = g === "x" ? f : (f & 3 | 8);
            return e.toString(16)
        })
    };
    a.show = function (e) {
        return new a(e).open()
    };
    a.alert = function () {
        var f = {};
        var e = {
            type: a.TYPE_PRIMARY,
            title: null,
            message: null,
            closable: false,
            draggable: false,
            buttonLabel: a.DEFAULT_TEXTS.OK,
            callback: null
        };
        if (typeof arguments[0] === "object" && arguments[0].constructor === {}.constructor) {
            f = d.extend(true, e, arguments[0])
        } else {
            f = d.extend(true, e, {
                message: arguments[0],
                callback: typeof arguments[1] !== "undefined" ? arguments[1] : null
            })
        }
        return new a({
            type: f.type,
            title: f.title,
            message: f.message,
            closable: f.closable,
            draggable: f.draggable,
            data: {
                callback: f.callback
            },
            onhide: function (g) {
                !g.getData("btnClicked") && g.isClosable() && typeof g.getData("callback") === "function" && g.getData("callback")(false)
            },
            buttons: [{
                label: f.buttonLabel,
                action: function (g) {
                    g.setData("btnClicked", true);
                    typeof g.getData("callback") === "function" && g.getData("callback")(true);
                    g.close()
                }
            }]
        }).open()
    };
    a.confirm = function () {
        var f = {};
        var e = {
            type: a.TYPE_PRIMARY,
            title: null,
            message: null,
            closable: false,
            draggable: false,
            btnCancelLabel: a.DEFAULT_TEXTS.CANCEL,
            btnOKLabel: a.DEFAULT_TEXTS.OK,
            btnOKClass: null,
            callback: null
        };
        if (typeof arguments[0] === "object" && arguments[0].constructor === {}.constructor) {
            f = d.extend(true, e, arguments[0])
        } else {
            f = d.extend(true, e, {
                message: arguments[0],
                closable: false,
                buttonLabel: a.DEFAULT_TEXTS.OK,
                callback: typeof arguments[1] !== "undefined" ? arguments[1] : null
            })
        }
        if (f.btnOKClass === null) {
            f.btnOKClass = ["btn", f.type.split("-")[1]].join("-")
        }
        return new a({
            type: f.type,
            title: f.title,
            message: f.message,
            closable: f.closable,
            draggable: f.draggable,
            data: {
                callback: f.callback
            },
            buttons: [{
                label: f.btnCancelLabel,
                action: function (g) {
                    typeof g.getData("callback") === "function" && g.getData("callback")(false);
                    g.close()
                }
            }, {
                label: f.btnOKLabel,
                cssClass: f.btnOKClass,
                action: function (g) {
                    typeof g.getData("callback") === "function" && g.getData("callback")(true);
                    g.close()
                }
            }]
        }).open()
    };
    a.warning = function (e, f) {
        return new a({
            type: a.TYPE_WARNING,
            message: e
        }).open()
    };
    a.danger = function (e, f) {
        return new a({
            type: a.TYPE_DANGER,
            message: e
        }).open()
    };
    a.success = function (e, f) {
        return new a({
            type: a.TYPE_SUCCESS,
            message: e
        }).open()
    };
    return a
}));
