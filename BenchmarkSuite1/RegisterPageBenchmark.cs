using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Library.Web.Areas.Identity.Pages.Account;
using Library.Web.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VSDiagnostics;

[CPUUsageDiagnoser]
public class RegisterPageBenchmark
{
    private RegisterModel _registerModel;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<IUserStore<ApplicationUser>> _userStoreMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<ILogger<RegisterModel>> _loggerMock;
    private Mock<IEmailSender> _emailSenderMock;
    private Mock<IUserEmailStore<ApplicationUser>> _emailStoreMock;
    [GlobalSetup]
    public void Setup()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
        _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _emailStoreMock = new Mock<IUserEmailStore<ApplicationUser>>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(_userManagerMock.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, null, null, null, null);
        _loggerMock = new Mock<ILogger<RegisterModel>>();
        _emailSenderMock = new Mock<IEmailSender>();
        // Setup mocks
        var authSchemes = new List<AuthenticationScheme>
        {
            new AuthenticationScheme("Google", "Google", typeof(object)),
            new AuthenticationScheme("Microsoft", "Microsoft", typeof(object))
        };
        _signInManagerMock.Setup(x => x.GetExternalAuthenticationSchemesAsync()).ReturnsAsync(authSchemes);
        _userStoreMock.Setup(x => x.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _emailStoreMock.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _userManagerMock.Setup(x => x.SupportsUserEmail).Returns(true);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
        _registerModel = new RegisterModel(_userManagerMock.Object, _userStoreMock.Object, _signInManagerMock.Object, _loggerMock.Object, _emailSenderMock.Object);
        var httpContextMock = new Mock<HttpContext>();
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(x => x.Scheme).Returns("https");
        httpContextMock.Setup(x => x.Request).Returns(requestMock.Object);
        var pageContextMock = new Mock<PageContext>();
        pageContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
        _registerModel.PageContext = pageContextMock.Object;
    }
