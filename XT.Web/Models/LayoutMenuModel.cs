using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.Model;
using XT.Web.External;

namespace XT.Web.Models
{
    public class MenuModel
    {
        public string icon { get; set; }
        public string name { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public string url { get; set; }
        public List<RoleTypeEnum> roles { get; set; }
        public List<MenuModel> children { get; set; }

        public MenuModel()
        { }

        public MenuModel(string _icon = "",
            string _name = "",
            string _action = "",
            string _controller = "",
            string _url = "",
            List<RoleTypeEnum> _roles = null,
            List<MenuModel> _children = null)
        {
            icon = _icon;
            name = _name;
            action = _action;
            controller = _controller;
            url = _url;
            roles = _roles;
            children = _children;
        }

        public bool Verify()
        {
            //manager
            if (XT.Web.External.AuthenticationManager.IsMod)
                return true;
            if (roles == null)
                return true;
            else
            {
                //everyone
                if (roles.Any(r => r == RoleTypeEnum.User))
                    return true;
                //it's me
                var my_role = AuthenticationManager.Role_Type_Id;
                if (roles.Any(r => (int)r == my_role))
                    return true;

                return false;
            }
        }

        public string GetUrl(UrlHelper Url, string _controller = "Admin")
        {
            if (controller == "")
                controller = _controller;
            var _url = url;
            if (_url == "")
            {
                _url = Url.Action(action, controller);
            }

            return _url;
        }
    }
}