using System;
using System.Diagnostics;
using ITI_ASP.NET_Project.Data;
using ITI_ASP.NET_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_ASP.NET_Project.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        static Person _person;
        private readonly ApplicationDbContext _db;

        public HomeController ( ILogger<HomeController> logger, ApplicationDbContext db ) {
            _logger = logger;
            _db = db;
        }

        #region Register 
        public IActionResult Register (  ) {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register ( Person p ) {
            if ( ModelState.IsValid ) {
                var user = _db.person.Where( p => p.UserName == p.UserName && p.Password == p.Password );
                if ( user != null ) {
                    _db.person.Add( p );
                    await _db.SaveChangesAsync();
                }

                if ( p.Type == "User" ) {
                    return RedirectToAction( "ViewProductsUser" );
                } else if ( p.Type == "Admin" ) {
                    return RedirectToAction( "ViewProductsAdmin" );
                }

                return RedirectToAction( "Index" );
            } else {
                return View();
            }
        }
        #endregion

        #region Login

        public IActionResult Login () {
            return View();
        }
        [HttpPost]
        public IActionResult Login ( Person person ) {
            
            _person =  _db.person.Where( p => p.UserName == person.UserName && p.Password == person.Password ).FirstOrDefault();
            if ( _person == null ) {
                return View();
            } else if ( _person.Type == "Admin" ) {
                return RedirectToAction( "ViewProductsAdmin" );
            } else {
                return RedirectToAction( "ViewProductsUser" );
            }
            
            return View();
        }

        #endregion

        public IActionResult Index () {
            var products = _db.Products.ToList();
            return View( products );
        }

        #region Admin

        #region view Products Admin
        public IActionResult ViewProductsAdmin () {
            var products = _db.Products.ToList();
            return View( products );
        }
        #endregion

        #region AddProduct
        public IActionResult AddProduct () {
            return View(  );
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct ( Product product, IFormFile Image ) {

            if ( Image != null && Image.Length > 0 ) {
                using ( var memoryStream = new MemoryStream() ) {
                    await Image.CopyToAsync( memoryStream );
                    product.ImageData = memoryStream.ToArray();
                    product.ContentType = Image.ContentType;
                }
            }

            _db.Products.Add( product );
            await _db.SaveChangesAsync();
            return RedirectToAction( "ViewProductsAdmin" );
        }
		#endregion

		#region Delete Product
		public IActionResult DeleteProduct ( int id ) {
			Product? product = _db.Products.FirstOrDefault( x => x.Id == id );
			_db.Products.Remove( product );
			_db.SaveChanges();
			return RedirectToAction( "ViewProductsAdmin" );
		}
		#endregion

		#region Edit Product
		public IActionResult EditProduct ( int id ) {
			Product product = _db.Products.FirstOrDefault( x => x.Id == id );
			return View( product );
		}

		[HttpPost]
		public async Task<IActionResult> EditProduct ( Product p, IFormFile Image ) {
			var product = await _db.Products.FindAsync( p.Id );
			if ( product == null ) {
				return NotFound();
			}

            product.Name = p.Name;
            product.Price = p.Price;

			if ( Image != null && Image.Length > 0 ) {
				using ( var memoryStream = new MemoryStream() ) {
					await Image.CopyToAsync( memoryStream );
                    product.ImageData = memoryStream.ToArray();
					product.ContentType = Image.ContentType;
				}
			}

			_db.Products.Update( product );
			await _db.SaveChangesAsync();
			return RedirectToAction( "ViewProductsAdmin" );
		}
        #endregion

        #endregion

        #region Admin Edit Users

        #region view Users by Admin
        public IActionResult ViewUsersAdmin () {
            var users = _db.person.ToList();
            return View( users );
        }
        #endregion

        #region Add User
        public IActionResult AddUser () {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser ( Person person ) {
            if ( ModelState.IsValid ) {
                var user = _db.person.Where( p => p.UserName == person.UserName && p.Password == person.Password );
                if ( user != null ) {
                    _db.person.Add( person );
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction( "ViewUsersAdmin" );
            } else {
			    return View();
            }
        }
        #endregion

        #region Delete User
        public IActionResult DeleteUser ( int id ) {
            Person? user = _db.person.FirstOrDefault( x => x.Id == id );
            _db.person.Remove( user );
            _db.SaveChanges();
            return RedirectToAction( "ViewUsersAdmin" );
        }
        #endregion

        #region Edit User
        public IActionResult EditUser ( int id ) {
            Person user = _db.person.FirstOrDefault( x => x.Id == id );
            return View( user );
        }

        [HttpPost]
        public async Task<IActionResult> EditUser ( Person p ) {
            if ( ModelState.IsValid ) {
                var user = await _db.person.FindAsync( p.Id );

                user.UserName = p.UserName;
                user.Password = p.Password;
                user.Type = p.Type;

                _db.person.Update( user );
                await _db.SaveChangesAsync();
                return RedirectToAction( "ViewUsersAdmin" );
			}else
            return View();
		}
        #endregion

        #endregion

        #region User

        #region view Products User
        public IActionResult ViewProductsUser () {
            var products = _db.Products.ToList();
            return View( products );
        }
		#endregion

		#region Cart

		public IActionResult Cart () {
			var cart = _db.cart.Where(i=>i.UserId == _person.Id).Include( c => c.product ).ToList();
			return View( cart );
		}
        
        #region Add to Cart
        public async Task<IActionResult> AddToCart ( int ProductId ) {

            Cart checkExistance = _db.cart.Include( c => c.product ).FirstOrDefault(x=>x.product.Id == ProductId);

            if ( checkExistance != null ) {
                checkExistance.Quantity += 1;
                _db.cart.Update( checkExistance );
                await _db.SaveChangesAsync();
            } else {
                Cart cartItem = new Cart();
			    Product p = _db.Products.FirstOrDefault( x => x.Id == ProductId );
               
                cartItem.Quantity = 1;
			    cartItem.UserId = _person.Id;
                cartItem.product = p;

                _db.cart.Add( cartItem );
                await _db.SaveChangesAsync();
            }
            return RedirectToAction( "Cart" );
        }
		#endregion
		
        #region Delete from Cart
		public IActionResult DeletefromCart ( int id ) {
			Cart? cart = _db.cart.FirstOrDefault( x => x.Id == id );
			_db.cart.Remove( cart );
			_db.SaveChanges();
			return RedirectToAction( "Cart" );
		}
		#endregion

		#endregion

		#endregion

		public IActionResult GetImage ( int id ) {
            var product = _db.Products.Find( id );
            if ( product == null || product.ImageData == null ) {
                return NotFound();
            }

            return File( product.ImageData, product.ContentType );
        }

        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error () {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
    }
}
