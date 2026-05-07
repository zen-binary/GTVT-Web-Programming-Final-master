using Project_Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project_Final.ViewComponents
{
    public class RenderViewComponent:ViewComponent
    {
        private List<MenuItem> menuItems = new List<MenuItem>();

        public RenderViewComponent()
        {
            menuItems = new List<MenuItem>()
            {
                new MenuItem() {Label="Tổng quan", Link="/Admin/Overview", Icon="ni ni-chart-bar-32 text-primary text-sm opacity-10"},
                new MenuItem() {Label="Danh mục", Link="/Admin/Category", Icon="ni ni-bullet-list-67 text-warning text-sm opacity-10"},
                new MenuItem() {Label="Sản phẩm", Link="/Admin/Product", Icon="ni ni-shop text-warning text-sm opacity-10"},
                new MenuItem() {Label="Bán hàng", Link="/Admin/Card", Icon = "ni ni-cart text-primary text-sm opacity-10"},
                new MenuItem() {Label="Hóa đơn", Link="/Admin/Bill", Icon = "ni ni-bullet-list-67 text-warning text-sm opacity-10"},
            };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderLeftMenu", menuItems);
        }
    }
}
