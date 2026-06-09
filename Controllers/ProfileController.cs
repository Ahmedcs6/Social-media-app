namespace mvc.Controllers;

public class ProfileController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public ProfileController(ApplicationDbContext context, IWebHostEnvironment WebHostEnvironment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _environment = WebHostEnvironment;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet("/Profile/{userName}")]
    public async Task<IActionResult> Index(string userName)
    {

        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
            return NotFound();
        var me = await _userManager.GetUserAsync(User);
        Follow? follow = null;
        if (me != null)
            follow = await _context.Follows.AsNoTracking().FirstOrDefaultAsync(f => f.FollowerId == me.Id && f.FollowingId == user.Id);
        bool IsFollowing = follow != null;
        ProfileVM profile = new ProfileVM
        {
            FullName = user.FullName,
            UserName = user.UserName,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            About = user.About,
            Address = user.Address,
            ImageUrl = user.ImageUrl,
            FollowersNumber = _context.Follows.Count(f => f.FollowingId == user.Id),
            FollowingsNumber = _context.Follows.Count(f => f.FollowerId == user.Id),
            IsFollowing = IsFollowing
        };
        return View(profile);
    }
    [Authorize]
    [HttpGet("Profile/Edit")]
    public async Task<IActionResult> Edit()
    {
        Console.WriteLine("========================================================================");
        var user = await _userManager.GetUserAsync(User);
        if (user == null) Console.WriteLine("User not found");
        if (user == null) return NotFound();

        var model = new EditVM
        {
            FirstName = user.FirstName,
            LasttName = user.LastName,
            UserName = user.UserName,
            BirthDate = user.BirthDate,
            About = user.About,
            Address = user.Address,
            Gender = user.Gender,
            ImageUrl = user.ImageUrl
        };

        return View(model);
    }
    [Authorize]
    [HttpPost("/Profile/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditVM model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) Console.WriteLine($"User Not Found");
            if (user == null) return NotFound();

            if (model.Image != null && model.Image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "images", "user", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await model.Image.CopyToAsync(stream);

                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, "images", "user", user.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                user.ImageUrl = fileName;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LasttName;
            user.UserName = model.UserName;
            user.BirthDate = model.BirthDate;
            user.About = model.About;
            user.Address = model.Address;
            user.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                model.ImageUrl = user.ImageUrl;
                return View(model);
            }
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Profile", new { username = user.UserName });
        }
        var currentUser = await _userManager.GetUserAsync(User);
        model.ImageUrl = currentUser?.ImageUrl;
        return View(model);
    }
    [Authorize]
    [HttpPost("/Profile/Follow/{userName}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Follow(string userName)
    {
        var me = await _userManager.GetUserAsync(User);
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        if (me == null || user == null)
            return Redirect($"/Profile/{userName}");

        var follow = new Follow
        {
            FollowerId = me.Id,
            FollowingId = user.Id
        };
        await _context.Follows.AddAsync(follow);
        await _context.SaveChangesAsync();
        return Redirect($"/Profile/{userName}");
    }
    [Authorize]
    [HttpPost("/Profile/Unfollow/{userName}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unfollow(string userName)
    {
        var me = await _userManager.GetUserAsync(User);
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        if (user == null || me == null)
            return Redirect($"/Profile/{userName}");
        var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == me.Id && f.FollowingId == user.Id);
        if (follow == null)
            return Redirect($"/Profile/{userName}");
        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
        return Redirect($"/Profile/{userName}");
    }
}

