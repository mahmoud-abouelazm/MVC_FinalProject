using Library.Web.Core.Models;

namespace Library.Web.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IConfiguration configuration;
        private readonly string logoUrl;
        private readonly string websiteUrl;
        private readonly string primaryColor = "#004b2a";
        private readonly string secondaryColor = "#046d3f";

        public EmailTemplateService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.logoUrl = "https://raw.githubusercontent.com/mahmoud-abouelazm/MVC_FinalProject/main/Library.Web/wwwroot/images/logo.png";
            this.websiteUrl = configuration["App:WebsiteUrl"] ?? "https://library-archive.com";
        }

        public async Task<string> GetRentalConfirmationEmailAsync(ApplicationUser user, List<Copy> copies, DateTime dueDate, decimal totalAmount)
        {
            var bookDetails = copies
                .GroupBy(c => c.Book)
                .Select(g => new { Book = g.Key, Count = g.Count() })
                .ToList();

            var booksList = string.Join("", bookDetails.Select(b =>
                $@"
                <tr style='border-bottom: 1px solid #e0e0e0;'>
                    <td style='padding: 12px; font-family: Arial, sans-serif; font-size: 14px; color: #1a1c1a;'>
                        <strong>{b.Book.Title}</strong>
                    </td>
                    <td style='padding: 12px; text-align: center; font-family: Arial, sans-serif; font-size: 14px; color: #1a1c1a;'>
                        {b.Count}
                    </td>
                    <td style='padding: 12px; text-align: right; font-family: Arial, sans-serif; font-size: 14px; color: #1a1c1a;'>
                        ${b.Book.Price.ToString("0.00")}
                    </td>
                </tr>"
            ));

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Rental Confirmation - The Editorial Archive</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #1a1c1a;
            background: linear-gradient(135deg, #f9faf6 0%, #f0f2ed 100%);
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(26, 28, 26, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, {primaryColor} 0%, {secondaryColor} 100%);
            color: #ffffff;
            padding: 40px 20px;
            text-align: center;
        }}
        .logo {{
            height: 60px;
            margin-bottom: 15px;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: 700;
            letter-spacing: 0.5px;
        }}
        .header p {{
            margin: 10px 0 0 0;
            font-size: 14px;
            opacity: 0.95;
            letter-spacing: 0.1em;
            text-transform: uppercase;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .greeting {{
            font-size: 16px;
            margin-bottom: 25px;
            color: #1a1c1a;
        }}
        .greeting strong {{
            color: {primaryColor};
        }}
        .section-title {{
            font-size: 16px;
            font-weight: 700;
            color: {primaryColor};
            margin: 30px 0 15px 0;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            border-bottom: 2px solid {primaryColor};
            padding-bottom: 10px;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }}
        th {{
            background: #f5f5f5;
            padding: 12px;
            text-align: left;
            font-weight: 700;
            color: #1a1c1a;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }}
        th:last-child {{
            text-align: right;
        }}
        td {{
            padding: 12px;
            font-size: 14px;
        }}
        .summary-box {{
            background: linear-gradient(135deg, #f0f2ed 0%, #e8eae5 100%);
            border-left: 4px solid {primaryColor};
            padding: 20px;
            margin: 25px 0;
            border-radius: 4px;
        }}
        .summary-row {{
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
            font-size: 15px;
        }}
        .summary-row.total {{
            border-top: 2px solid {primaryColor};
            padding-top: 10px;
            margin-top: 10px;
            font-size: 18px;
            font-weight: 700;
            color: {primaryColor};
        }}
        .due-date {{
            background: #fffbf0;
            border: 1px solid #ffd599;
            color: #d97706;
            padding: 15px;
            border-radius: 6px;
            margin: 20px 0;
            text-align: center;
            font-weight: 600;
        }}
        .info-box {{
            background: #f0f7f4;
            border: 1px solid #a8d5ba;
            color: #046d3f;
            padding: 15px;
            border-radius: 6px;
            margin: 20px 0;
            font-size: 14px;
            line-height: 1.6;
        }}
        .cta-button {{
            display: inline-block;
            background: linear-gradient(135deg, {primaryColor} 0%, {secondaryColor} 100%);
            color: #ffffff;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 6px;
            font-weight: 600;
            margin: 20px 0;
            text-align: center;
        }}
        .footer {{
            background: #f5f5f5;
            padding: 25px;
            text-align: center;
            border-top: 1px solid #e0e0e0;
        }}
        .footer p {{
            margin: 5px 0;
            font-size: 12px;
            color: #666;
        }}
        .footer-link {{
            color: {primaryColor};
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='{logoUrl}' alt='The Editorial Archive' class='logo' />
            <h1>Rental Confirmation</h1>
            <p>📚 The Editorial Archive 📚</p>
        </div>

        <div class='content'>
            <div class='greeting'>
                Hello <strong>{user.FullName}</strong>,
            </div>

            <p>Your rental request has been successfully processed. Thank you for choosing The Editorial Archive!</p>

            <div class='section-title'>📖 Rental Details</div>

            <table>
                <thead>
                    <tr>
                        <th>Book Title</th>
                        <th>Copies</th>
                        <th>Price/Day</th>
                    </tr>
                </thead>
                <tbody>
                    {booksList}
                </tbody>
            </table>

            <div class='due-date'>
                📅 Due Date: <strong>{dueDate:MMMM d, yyyy}</strong>
            </div>

            <div class='summary-box'>
                <div class='summary-row'>
                    <span>Books Rented:</span>
                    <span>{copies.Count} copies</span>
                </div>
                <div class='summary-row'>
                    <span>Rental Period:</span>
                    <span>{(int)Math.Floor((dueDate - DateTime.UtcNow).TotalDays) + 1} days</span>
                </div>
                <div class='summary-row total'>
                    <span>Total Amount:</span>
                    <span>${totalAmount:0.00}</span>
                </div>
            </div>

            <div class='info-box'>
                <strong>📌 Important Reminders:</strong>
                <ul style='margin: 10px 0; padding-left: 20px;'>
                    <li>Please return books by the due date to avoid late fees (2x price per day)</li>
                    <li>Late return charges will be automatically calculated upon return</li>
                    <li>Handle books with care - damaged books may incur additional fees</li>
                    <li>You can re-rent books from your profile</li>
                </ul>
            </div>

            <div style='text-align: center;'>
                <a href='{websiteUrl}/Identity/Manage/Profile' class='cta-button'>View My Rentals</a>
            </div>

            <p style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 13px; color: #666;'>
                If you have any questions, please don't hesitate to contact our support team.
            </p>
        </div>

        <div class='footer'>
            <p><strong>The Editorial Archive</strong></p>
            <p>Your gateway to knowledge and discovery</p>
            <p style='margin-top: 15px;'>
                <a href='{websiteUrl}' class='footer-link'>Visit Website</a> | 
                <a href='{websiteUrl}/Identity/Manage/Profile' class='footer-link'>My Profile</a>
            </p>
            <p style='margin-top: 15px; color: #999;'>
                © 2024 The Editorial Archive. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";
        }

        public async Task<string> GetReturnInvoiceEmailAsync(ApplicationUser user, Rental rental)
        {
            var returnedAt = rental.ReturnedAt ?? DateTime.UtcNow;
            var daysRented = Math.Max(1, (int)Math.Ceiling((returnedAt - rental.RentedAt).TotalDays));
            var totalPricePerDay = rental.CopyRentals.Sum(cr => cr.Copy?.Book?.Price ?? 0);
            var isLate = returnedAt > rental.DueAt;
            var daysLate = isLate ? (int)Math.Ceiling((returnedAt - rental.DueAt).TotalDays) : 0;
            var onTimeDays = Math.Max(0, daysRented - daysLate);
            var baseAmount = onTimeDays * totalPricePerDay;
            var penaltyAmount = isLate ? daysLate * totalPricePerDay * 2 : 0;
            var totalAmount = rental.Amount > 0 ? rental.Amount : baseAmount + penaltyAmount;

            var bookTitles = string.Join(", ", rental.CopyRentals.Select(cr => cr.Copy?.Book?.Title).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct());

            var penaltySection = isLate ? $@"
                <tr style='background: #fee2e2; border-bottom: 1px solid #fca5a5;'>
                    <td style='padding: 12px; font-family: Arial, sans-serif; font-size: 14px; color: #dc2626;'>
                        <strong>⚠️ Late Fees ({daysLate} day{(daysLate > 1 ? "s" : "")})</strong>
                    </td>
                    <td style='padding: 12px; text-align: right; font-family: Arial, sans-serif; font-size: 14px; color: #dc2626;'>
                        <strong>+${penaltyAmount:0.00}</strong>
                    </td>
                </tr>" : "";

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Return Invoice - The Editorial Archive</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #1a1c1a;
            background: linear-gradient(135deg, #f9faf6 0%, #f0f2ed 100%);
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(26, 28, 26, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, {primaryColor} 0%, {secondaryColor} 100%);
            color: #ffffff;
            padding: 40px 20px;
            text-align: center;
        }}
        .logo {{
            height: 60px;
            margin-bottom: 15px;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: 700;
            letter-spacing: 0.5px;
        }}
        .header p {{
            margin: 10px 0 0 0;
            font-size: 14px;
            opacity: 0.95;
            letter-spacing: 0.1em;
            text-transform: uppercase;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .invoice-header {{
            display: flex;
            justify-content: space-between;
            align-items: start;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid {primaryColor};
        }}
        .invoice-title h2 {{
            margin: 0;
            font-size: 22px;
            color: {primaryColor};
            font-weight: 700;
        }}
        .invoice-number {{
            text-align: right;
        }}
        .invoice-number p {{
            margin: 2px 0;
            font-size: 13px;
            color: #666;
        }}
        .invoice-number strong {{
            font-size: 16px;
            color: {primaryColor};
        }}
        .section-title {{
            font-size: 14px;
            font-weight: 700;
            color: {primaryColor};
            margin: 25px 0 15px 0;
            text-transform: uppercase;
            letter-spacing: 0.1em;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin: 15px 0;
        }}
        th {{
            background: #f5f5f5;
            padding: 12px;
            text-align: left;
            font-weight: 700;
            color: #1a1c1a;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.05em;
            border-bottom: 2px solid {primaryColor};
        }}
        th:last-child {{
            text-align: right;
        }}
        td {{
            padding: 12px;
            font-size: 14px;
            border-bottom: 1px solid #e0e0e0;
        }}
        .total-row {{
            background: linear-gradient(135deg, #f0f2ed 0%, #e8eae5 100%);
            font-weight: 700;
            font-size: 16px;
            color: {primaryColor};
        }}
        .total-row td {{
            padding: 15px 12px;
        }}
        .detail-row {{
            display: flex;
            justify-content: space-between;
            padding: 10px 0;
            border-bottom: 1px solid #e0e0e0;
            font-size: 14px;
        }}
        .detail-row strong {{
            color: {primaryColor};
        }}
        .detail-box {{
            background: #f9faf6;
            padding: 20px;
            border-radius: 6px;
            margin: 20px 0;
        }}
        .late-warning {{
            background: #fee2e2;
            border-left: 4px solid #dc2626;
            color: #dc2626;
            padding: 15px;
            border-radius: 4px;
            margin: 20px 0;
            font-weight: 600;
        }}
        .footer {{
            background: #f5f5f5;
            padding: 25px;
            text-align: center;
            border-top: 1px solid #e0e0e0;
        }}
        .footer p {{
            margin: 5px 0;
            font-size: 12px;
            color: #666;
        }}
        .footer-link {{
            color: {primaryColor};
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='{logoUrl}' alt='The Editorial Archive' class='logo' />
            <h1>Return Invoice</h1>
            <p>📚 The Editorial Archive 📚</p>
        </div>

        <div class='content'>
            <div class='invoice-header'>
                <div class='invoice-title'>
                    <h2>Invoice #{rental.Id}</h2>
                </div>
                <div class='invoice-number'>
                    <p><strong>Date:</strong> {returnedAt:MMMM d, yyyy}</p>
                    <p><strong>Status:</strong> Returned</p>
                </div>
            </div>

            <div class='section-title'>📋 Rental Information</div>
            <div class='detail-box'>
                <div class='detail-row'>
                    <span>Customer:</span>
                    <strong>{user.FullName}</strong>
                </div>
                <div class='detail-row'>
                    <span>Email:</span>
                    <strong>{user.Email}</strong>
                </div>
                <div class='detail-row'>
                    <span>Rental ID:</span>
                    <strong>#{rental.Id}</strong>
                </div>
                <div class='detail-row'>
                    <span>Books:</span>
                    <strong>{bookTitles}</strong>
                </div>
                <div class='detail-row'>
                    <span>Rented On:</span>
                    <strong>{rental.RentedAt:MMMM d, yyyy}</strong>
                </div>
                <div class='detail-row'>
                    <span>Due Date:</span>
                    <strong style='color: {(isLate ? "#dc2626" : primaryColor)}'>{rental.DueAt:MMMM d, yyyy}</strong>
                </div>
                <div class='detail-row'>
                    <span>Returned On:</span>
                    <strong>{returnedAt:MMMM d, yyyy}</strong>
                </div>
            </div>

            {(isLate ? $@"<div class='late-warning'>⚠️ This rental was returned {daysLate} day{(daysLate > 1 ? "s" : "")} late. Late fees have been applied.</div>" : "")}

            <div class='section-title'>💰 Amount Breakdown</div>
            <table>
                <thead>
                    <tr>
                        <th>Description</th>
                        <th style='text-align: right;'>Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>{onTimeDays} day{(onTimeDays == 1 ? "" : "s")} on time × ${totalPricePerDay:0.00}/day</td>
                        <td style='text-align: right;'><strong>${baseAmount:0.00}</strong></td>
                    </tr>
                    {penaltySection}
                    <tr class='total-row'>
                        <td>Total Amount</td>
                        <td style='text-align: right;'>${totalAmount:0.00}</td>
                    </tr>
                </tbody>
            </table>

            <div class='detail-box'>
                <p style='margin: 0; font-size: 13px; color: #666;'>
                    <strong>Thank you for returning your books on time!</strong> We appreciate your patronage and look forward to serving you again.
                </p>
            </div>

            <p style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 13px; color: #666;'>
                Need help? Contact our support team or visit our website.
            </p>
        </div>

        <div class='footer'>
            <p><strong>The Editorial Archive</strong></p>
            <p>Your gateway to knowledge and discovery</p>
            <p style='margin-top: 15px;'>
                <a href='{websiteUrl}' class='footer-link'>Visit Website</a> | 
                <a href='{websiteUrl}/Identity/Manage/Profile' class='footer-link'>My Profile</a>
            </p>
            <p style='margin-top: 15px; color: #999;'>
                © 2024 The Editorial Archive. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";
        }

        public async Task<string> GetRegistrationWelcomeEmailAsync(ApplicationUser user)
        {
            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Welcome to The Editorial Archive</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #1a1c1a;
            background: linear-gradient(135deg, #f9faf6 0%, #f0f2ed 100%);
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(26, 28, 26, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, {primaryColor} 0%, {secondaryColor} 100%);
            color: #ffffff;
            padding: 50px 20px;
            text-align: center;
        }}
        .logo {{
            height: 70px;
            margin-bottom: 20px;
        }}
        .header h1 {{
            margin: 0;
            font-size: 32px;
            font-weight: 700;
            letter-spacing: 0.5px;
        }}
        .header p {{
            margin: 15px 0 0 0;
            font-size: 16px;
            opacity: 0.95;
            letter-spacing: 0.1em;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .greeting {{
            font-size: 18px;
            margin-bottom: 25px;
            color: #1a1c1a;
            line-height: 1.8;
        }}
        .greeting strong {{
            color: {primaryColor};
        }}
        .section-title {{
            font-size: 16px;
            font-weight: 700;
            color: {primaryColor};
            margin: 30px 0 15px 0;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            border-bottom: 2px solid {primaryColor};
            padding-bottom: 10px;
        }}
        .feature-list {{
            list-style: none;
            padding: 0;
            margin: 20px 0;
        }}
        .feature-list li {{
            padding: 12px 0;
            padding-left: 30px;
            position: relative;
            font-size: 14px;
            color: #1a1c1a;
            line-height: 1.6;
        }}
        .feature-list li:before {{
            content: '✓';
            position: absolute;
            left: 0;
            color: {primaryColor};
            font-weight: bold;
            font-size: 18px;
        }}
        .highlight-box {{
            background: linear-gradient(135deg, #f0f2ed 0%, #e8eae5 100%);
            border-left: 4px solid {primaryColor};
            padding: 20px;
            margin: 25px 0;
            border-radius: 4px;
        }}
        .highlight-box h3 {{
            margin: 0 0 10px 0;
            color: {primaryColor};
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 0.1em;
        }}
        .cta-button {{
            display: inline-block;
            background: linear-gradient(135deg, {primaryColor} 0%, {secondaryColor} 100%);
            color: #ffffff;
            padding: 14px 40px;
            text-decoration: none;
            border-radius: 6px;
            font-weight: 600;
            margin: 25px 0;
            text-align: center;
            font-size: 16px;
        }}
        .info-grid {{
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 15px;
            margin: 25px 0;
        }}
        .info-card {{
            background: #f9faf6;
            padding: 15px;
            border-radius: 6px;
            border: 1px solid #e0e0e0;
        }}
        .info-card strong {{
            color: {primaryColor};
            display: block;
            margin-bottom: 5px;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }}
        .info-card p {{
            margin: 0;
            font-size: 13px;
            color: #666;
        }}
        .footer {{
            background: #f5f5f5;
            padding: 25px;
            text-align: center;
            border-top: 1px solid #e0e0e0;
        }}
        .footer p {{
            margin: 5px 0;
            font-size: 12px;
            color: #666;
        }}
        .footer-link {{
            color: {primaryColor};
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='{logoUrl}' alt='The Editorial Archive' class='logo' />
            <h1>Welcome!</h1>
            <p>📚 JOIN THE EDITORIAL ARCHIVE 📚</p>
        </div>

        <div class='content'>
            <div class='greeting'>
                Hello <strong>{user.FullName}</strong>,
                <br><br>
                Thank you for joining The Editorial Archive! We're thrilled to have you as part of our community of knowledge seekers and book enthusiasts.
            </div>

            <div class='section-title'>🎉 You're All Set!</div>
            <p>Your account has been successfully created and is ready to use. You can now browse our extensive collection of books and start renting today.</p>

            <div class='highlight-box'>
                <h3>What You Can Do Now</h3>
                <ul class='feature-list'>
                    <li>Browse our curated collection of thousands of books</li>
                    <li>Search by author, category, or title</li>
                    <li>Rent books for up to 30 days</li>
                    <li>Track your active rentals and rental history</li>
                    <li>Explore author profiles and book recommendations</li>
                    <li>Manage your account and preferences</li>
                </ul>
            </div>

            <div class='section-title'>📖 Getting Started</div>
            <div class='info-grid'>
                <div class='info-card'>
                    <strong>1. Browse</strong>
                    <p>Explore our catalog and find books that interest you</p>
                </div>
                <div class='info-card'>
                    <strong>2. Select</strong>
                    <p>Choose your desired copies and rental period</p>
                </div>
                <div class='info-card'>
                    <strong>3. Rent</strong>
                    <p>Complete the rental process and enjoy</p>
                </div>
                <div class='info-card'>
                    <strong>4. Return</strong>
                    <p>Return by the due date to avoid fees</p>
                </div>
            </div>

            <div class='highlight-box'>
                <h3>💡 Pro Tips</h3>
                <ul class='feature-list'>
                    <li>Set a reminder for your due dates to avoid late fees</li>
                    <li>Check out author profiles to discover similar books</li>
                    <li>Take advantage of our category filters for easier browsing</li>
                    <li>Late fees are 2x the rental price per day</li>
                </ul>
            </div>

            <div style='text-align: center;'>
                <a href='{websiteUrl}/Books' class='cta-button'>Start Browsing Books</a>
            </div>

            <div class='info-grid' style='margin-top: 30px;'>
                <div class='info-card'>
                    <strong>Your Account Email</strong>
                    <p>{user.Email}</p>
                </div>
                <div class='info-card'>
                    <strong>Account Status</strong>
                    <p>Active & Ready</p>
                </div>
            </div>

            <p style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 13px; color: #666;'>
                If you have any questions or need assistance, don't hesitate to reach out to our support team. Happy reading!
            </p>
        </div>

        <div class='footer'>
            <p><strong>The Editorial Archive</strong></p>
            <p>Your gateway to knowledge and discovery</p>
            <p style='margin-top: 15px;'>
                <a href='{websiteUrl}' class='footer-link'>Visit Website</a> | 
                <a href='{websiteUrl}/Books' class='footer-link'>Browse Books</a> | 
                <a href='{websiteUrl}/Home/Authors' class='footer-link'>Explore Authors</a>
            </p>
            <p style='margin-top: 15px; color: #999;'>
                © 2024 The Editorial Archive. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";
        }

        public async Task<string> GetOverdueReminderEmailAsync(ApplicationUser user, Rental rental)
        {
            var daysOverdue = Math.Max(1, (int)Math.Ceiling((DateTime.UtcNow - rental.DueAt).TotalDays));
            var dailyPenalty = rental.CopyRentals.Sum(cr => cr.Copy?.Book?.Price ?? 0);
            var accumulatedPenalty = daysOverdue * dailyPenalty * 2;

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Rental Overdue Reminder - The Editorial Archive</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #1a1c1a;
            background: linear-gradient(135deg, #f9faf6 0%, #f0f2ed 100%);
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(26, 28, 26, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #dc2626 0%, #b91c1c 100%);
            color: #ffffff;
            padding: 40px 20px;
            text-align: center;
        }}
        .logo {{
            height: 60px;
            margin-bottom: 15px;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: 700;
            letter-spacing: 0.5px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .alert-box {{
            background: #fee2e2;
            border: 2px solid #dc2626;
            color: #dc2626;
            padding: 20px;
            border-radius: 6px;
            margin: 20px 0;
            text-align: center;
        }}
        .alert-box strong {{
            font-size: 18px;
            display: block;
            margin: 10px 0;
        }}
        .section-title {{
            font-size: 16px;
            font-weight: 700;
            color: #dc2626;
            margin: 25px 0 15px 0;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            border-bottom: 2px solid #dc2626;
            padding-bottom: 10px;
        }}
        .detail-box {{
            background: #f9faf6;
            padding: 20px;
            border-radius: 6px;
            margin: 20px 0;
        }}
        .detail-row {{
            display: flex;
            justify-content: space-between;
            padding: 10px 0;
            border-bottom: 1px solid #e0e0e0;
            font-size: 14px;
        }}
        .detail-row:last-child {{
            border-bottom: none;
        }}
        .detail-row strong {{
            color: #dc2626;
        }}
        .cta-button {{
            display: inline-block;
            background: #dc2626;
            color: #ffffff;
            padding: 14px 40px;
            text-decoration: none;
            border-radius: 6px;
            font-weight: 600;
            margin: 25px 0;
            text-align: center;
            font-size: 16px;
        }}
        .footer {{
            background: #f5f5f5;
            padding: 25px;
            text-align: center;
            border-top: 1px solid #e0e0e0;
        }}
        .footer p {{
            margin: 5px 0;
            font-size: 12px;
            color: #666;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='{logoUrl}' alt='The Editorial Archive' class='logo' />
            <h1>⚠️ Overdue Reminder</h1>
        </div>

        <div class='content'>
            <div class='alert-box'>
                <p>Your rental is now <strong>{daysOverdue} day{(daysOverdue > 1 ? "s" : "")} overdue!</strong></p>
                <p>Please return your books immediately to avoid additional charges.</p>
            </div>

            <p>Hi {user.FullName},</p>

            <p>We noticed that your rental is still outstanding. Late fees are accumulating daily at 2x the rental price.</p>

            <div class='section-title'>📚 Rental Details</div>
            <div class='detail-box'>
                <div class='detail-row'>
                    <span>Rental ID:</span>
                    <strong>#{rental.Id}</strong>
                </div>
                <div class='detail-row'>
                    <span>Due Date:</span>
                    <strong>{rental.DueAt:MMMM d, yyyy}</strong>
                </div>
                <div class='detail-row'>
                    <span>Days Overdue:</span>
                    <strong>{daysOverdue}</strong>
                </div>
                <div class='detail-row'>
                    <span>Accumulated Penalty:</span>
                    <strong>${accumulatedPenalty:0.00}</strong>
                </div>
            </div>

            <div style='background: #fee2e2; border-left: 4px solid #dc2626; padding: 15px; border-radius: 4px; margin: 20px 0;'>
                <p style='margin: 0; color: #dc2626;'><strong>⏰ Please return your books today to minimize additional fees!</strong></p>
            </div>

            <div style='text-align: center;'>
                <a href='{websiteUrl}/Identity/Manage/Profile' class='cta-button'>View My Rentals</a>
            </div>

            <p>If you have any questions or need assistance, please contact our support team immediately.</p>
        </div>

        <div class='footer'>
            <p><strong>The Editorial Archive</strong></p>
            <p>Your gateway to knowledge and discovery</p>
            <p style='margin-top: 15px; color: #999;'>
                © 2024 The Editorial Archive. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
