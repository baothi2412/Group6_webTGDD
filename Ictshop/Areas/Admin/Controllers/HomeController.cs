using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;
using PagedList;
using Xceed.Words.NET;

namespace Ictshop.Areas.Admin.Controllers
{
    public class HomeController : Controller

    {
        QLdienthoaiEntities db = new QLdienthoaiEntities();

        // GET: Admin/Home

        public ActionResult Index(int? page)
        {
            // 1. Tham số int? dùng để thể hiện null và kiểu int( số nguyên)
            // page có thể có giá trị là null ( rỗng) và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn sql, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo Masp mới có thể phân trang.
            var sp = db.Sanphams.OrderBy(x => x.Masp);

            // 4. Tạo kích thước trang (pageSize) hay là số sản phẩm hiển thị trên 1 trang
            int pageSize = 5;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các sản phẩm được phân trang theo kích thước và số trang.
            return View(sp.ToPagedList(pageNumber, pageSize));

        }

        // Xem chi tiết người dùng GET: Admin/Home/Details/5 
        public ActionResult Details(int id)
        {
            var dt = db.Sanphams.Find(id);
            return View(dt);
        }

        // Tạo sản phẩm mới phương thức GET: Admin/Home/Create
        public ActionResult Create()
        {
            //Để tạo dropdownList bên view
            var hangselected = new SelectList(db.Hangsanxuats, "Mahang", "Tenhang");
            ViewBag.Mahang = hangselected;
            var hdhselected = new SelectList(db.Hedieuhanhs, "Mahdh", "Tenhdh");
            ViewBag.Mahdh = hdhselected;
            return View();
        }

        // Tạo sản phẩm mới phương thức POST: Admin/Home/Create
        [HttpPost]

      
        public ActionResult Create(Sanpham sanpham, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    // Generate a unique filename based on Masp and the original file extension
                    string filename = $"{sanpham.Masp}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                    // Combine the path with the generated filename
                    string path = Path.Combine(Server.MapPath("~/HinhanhSP/"), filename);

                    // Save the uploaded file to the specified path
                    image.SaveAs(path);

                    // Set the Anhbia property of the Sanpham object to the generated filename
                    sanpham.Anhbia = filename;
                }

                // Add the Sanpham object to the database and save changes
                db.Sanphams.Add(sanpham);
                db.SaveChanges();

                // Redirect to the Index action
                return RedirectToAction("Index");
            }

            // If the ModelState is not valid, return the view with the model
            return View(sanpham);
        }



        // Sửa sản phẩm GET lấy ra ID sản phẩm: Admin/Home/Edit/5
        public ActionResult Edit(int id)
        {
            // Hiển thị dropdownlist
            var dt = db.Sanphams.Find(id);
            var hangselected = new SelectList(db.Hangsanxuats, "Mahang", "Tenhang", dt.Mahang);
            ViewBag.Mahang = hangselected;
            var hdhselected = new SelectList(db.Hedieuhanhs, "Mahdh", "Tenhdh", dt.Mahdh);
            ViewBag.Mahdh = hdhselected;

            return View(dt);
        }

        // POST: Admin/Home/Edit/5
        [HttpPost]
        public ActionResult Edit(Sanpham sanpham, HttpPostedFileBase image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        // Generate a unique filename based on Masp and the original file extension
                        string filename = $"{sanpham.Masp}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                        // Combine the path with the generated filename
                        string path = Path.Combine(Server.MapPath("~/HinhanhSP/"), filename);

                        // Save the uploaded file to the specified path
                        image.SaveAs(path);

                        // Set the Anhbia property of the Sanpham object to the generated filename
                        sanpham.Anhbia = filename;
                    }

                    // Sửa sản phẩm theo mã sản phẩm
                    var oldItem = db.Sanphams.Find(sanpham.Masp);
                    oldItem.Tensp = sanpham.Tensp;
                    oldItem.Giatien = sanpham.Giatien;
                    oldItem.Soluong = sanpham.Soluong;
                    oldItem.Mota = sanpham.Mota;
                    oldItem.Bonhotrong = sanpham.Bonhotrong;
                    oldItem.Ram = sanpham.Ram;
                    oldItem.Thesim = sanpham.Thesim;
                    oldItem.Mahang = sanpham.Mahang;
                    oldItem.Mahdh = sanpham.Mahdh;

                    // Update the Anhbia property only if a new image is uploaded
                    if (image != null && image.ContentLength > 0)
                    {
                        oldItem.Anhbia = sanpham.Anhbia;
                    }

                    // Lưu lại
                    db.SaveChanges();

                    // Redirect to the Index action
                    return RedirectToAction("Index");
                }

                // If the ModelState is not valid, return the view with the model
                var hangselected = new SelectList(db.Hangsanxuats, "Mahang", "Tenhang", sanpham.Mahang);
                ViewBag.Mahang = hangselected;
                var hdhselected = new SelectList(db.Hedieuhanhs, "Mahdh", "Tenhdh", sanpham.Mahdh);
                ViewBag.Mahdh = hdhselected;
                return View(sanpham);
            }
            catch
            {
                return View();
            }
        }

       
    



// Xoá sản phẩm phương thức GET: Admin/Home/Delete/5
public ActionResult Delete(int id)
        {
            var dt = db.Sanphams.Find(id);
            return View(dt);
        }

        // Xoá sản phẩm phương thức POST: Admin/Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                //Lấy được thông tin sản phẩm theo ID(mã sản phẩm)
                var dt = db.Sanphams.Find(id);
                // Xoá
                db.Sanphams.Remove(dt);
                // Lưu lại
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
      

    }
}
