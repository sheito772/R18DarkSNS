// ...前略
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
.AddTwitter(twitterOptions =>
{
    twitterOptions.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"];
    twitterOptions.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];
})
.AddApple(options =>
{
    options.ClientId = builder.Configuration["Authentication:Apple:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Apple:ClientSecret"];
    options.UsePrivateKey(builder.Configuration["Authentication:Apple:PrivateKey"]);
    options.KeyId = builder.Configuration["Authentication:Apple:KeyId"];
    options.TeamId = builder.Configuration["Authentication:Apple:TeamId"];
})
.AddLine(options =>
{
    options.ChannelId = builder.Configuration["Authentication:Line:ChannelId"];
    options.ChannelSecret = builder.Configuration["Authentication:Line:ChannelSecret"];
});

// ...後略
