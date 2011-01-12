/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; version 2 of the License only.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
* USA
*/

using System;
using System.Text;
using System.Windows.Forms;

namespace Vha.Chat.Commands
{
    public class DonateCommand : Command
    {
        public override bool Process(Context context, string trigger, string message, string[] args)
        {
            string email = "paypal@vhabot.net";
            string description = "Vha.Chat%20appreciation%20donation";
            string country = "US";
            string currency = "EUR";

            string url = string.Format(
                "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business={0}&lc={1}&item_name={2}&currency_code={3}&bn=PP%2dDonationsBF",
                email, country, description, currency);

            System.Diagnostics.Process.Start(url);
            return true;
        }

        public DonateCommand()
            : base(
                "Donation", // Name
                new string[] { "donate" }, // Triggers
                new string[] { "donate" }, // Usage
                new string[] { "donate" }, // Examples
                // Description
                "Launches a link to donate money to the author of this application through the use of PayPal"
            )
        { }
    }
}
