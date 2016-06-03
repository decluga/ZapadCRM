var App = App || {};

/** TODO - refactor */
App.Message = {
    /** Get service message */
    getServiceMessage: function (type, message) {
        var serviceMessage = '<div class="alert alert-' + type + '" role="alert">' + message + '</div>';

        return serviceMessage;
    },

    /** Clear previous service message */
    clearServiceMessage: function ($parentElement) {
        $parentElement.html("");
    },

    /** Show service message */
    showServiceMessage: function (type, message, $parentElement, clearServiceMessage) {
        /** Clear previous service message if need */
        var clearServiceMessage = (clearServiceMessage === false) ? false : true;

        /** Get parent service message */
        var $parentServiceMessage = $parentElement.find(".parent-service-message");

        if (clearServiceMessage) {
            App.Message.clearServiceMessage($parentServiceMessage);
        }

        /** Get service message */
        var serviceMessage = App.Message.getServiceMessage(type, message);

        /** Append service message to parent */
        $parentServiceMessage.append(serviceMessage);
    },

    /** Show success message */
    showSuccessMessage: function (message, $parentElement, clearServiceMessage) {
        var type = "success";

        App.Message.showServiceMessage(type, message, $parentElement, clearServiceMessage);
    },

    /** Show error message */
    showErrorMessage: function (message, $parentElement, clearServiceMessage) {
        var type = "danger";

        App.Message.showServiceMessage(type, message, $parentElement, clearServiceMessage);
    },

    hideMessage: function ($parentElement) {
        /** Get parent service message */
        var $parentServiceMessage = $parentElement.find(".parent-service-message");

        App.Message.clearServiceMessage($parentServiceMessage);
    }
};