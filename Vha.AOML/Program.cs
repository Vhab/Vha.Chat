using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Vha.AOML
{
    internal static class Program
    {
        // Application entry
        static void Main(string[] args)
        {
            string data = "<test /><noes><font color=\"<font color=\"green\" grumble wobble=red color=orange><font color = \'red\'>888<font color=blue>999<center>123<right>3</font>4</font>5<div /></div>6</center><br></br>< img src=\"123\" />678";
            Parse(data);

            data = "<font color=#79CBE6>Your query <font color=#FFFFFF>combined</font> has returned <font color=#FFFFFF>65</font> results\n" +
                "  <font color=#79CBE6>Blackmane's Combined Officer's Headwear <font color=#CCCCCC>[</font><a href='itemref://257383/257383/300'>300</a><font color=#CCCCCC>] </font></font>\n" +
                "  <font color=#79CBE6>Combined Bio Samples - Stage Four <font color=#CCCCCC>[</font><a href='itemref://267777/267777/250'>250</a><font color=#CCCCCC>] </font></font>\n" +
                "  <font color=#79CBE6>Combined Bio Samples - Stage One <font color=#CCCCCC>[</font><a href='itemref://267774/267774/250'>250</a><font color=#CCCCCC>] </font></font>\n" +
                "  <font color=#79CBE6>Combined Bio Samples - Stage Three <font color=#CCCCCC>[</font><a href='itemref://267776/267776/250'>250</a><font color=#CCCCCC>] </font></font>\n" +
                "  <font color=#79CBE6>Combined Bio Samples - Stage Two <font color=#CCCCCC>[</font><a href='itemref://267775/267775/250'>250</a><font color=#CCCCCC>] </font></font>\n" +
                "[vhabbots:Vhabion@chat.d1.funcom.com:7101] [Itemsbot]: <font color=#79CBE6>More Results »» <a href=\"text://<font color=#FFFFFF><font color=#3C8799>:::::::::::</font> VhaBot Client Terminal <font color=#3C8799>:::::::::::</font>\n" +
                "<font color=#3C8799>« </font><a href='chatcmd:///tell Itemsbot version' style='text-decoration:none'>About</a><font color=#3C8799> »     « </font><a href='chatcmd:///tell Itemsbot help' style='text-decoration:none'>Help</a><font color=#3C8799> »     « </font><a href='chatcmd:///close InfoView' style='text-decoration:none'>Close Terminal</a><font color=#3C8799> »</font>\n" +
                "</font><font color=#3C8799>¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯</font>\">clickme";
            data = "[Clan OOC] Flikfak: <a href=\"text://  <img src=rdb://214739> <font color=#FF3300>My Tradeskill Service</font>  <img src=rdb://214739><br> <br>Mech. Engi     --> 2100<br>Elec. Engi      --> 2100<br>Quantum FT   --> 2100<br>Weapon Smt  --> 2100<br>Pharma Tech  --> 1950</font><br>Nano Program --> 1800<br>Comp. Liter    --> 1800<br>Psychology    --> 1650<br>Chemistry      --> 2100<br>Tutoring        --> 1600<br>Break&Entry   --> 1250<br><br>All Tredaskills without city<br><br><a href='chatcmd:///tell Flikfak Master, i need your help>Tell Flik</a> or get <a href='chatcmd:///fxscript EP03_Orbital_Attack #\n /fxscript EP03_Bomber_Event_01 #\n /text Boom>Nuked!</a>\">Need a Builder?</a> Engi @ Missi Term Nord</a>";
            //Parse(data);
            Console.ReadLine();
        }

        static void Parse(string aoml)
        {
            Parser p = new Parser();
            p.Mode = ParserMode.Compatibility;
            NodeCollection n = p.Parse(aoml);
            p.Sanitize(n);
            p.Balance(n);

            foreach (Node node in n)
            {
                // Output node
                switch (node.Type)
                {
                    case NodeType.Open:
                        OpenNode open = (OpenNode)node;
                        Console.Write("Open");
                        if (open.Closed) Console.Write("Close");
                        Console.Write(": " + open.Name + " [ ");
                        for (int i = 0; i < open.Count; i++)
                        {
                            string attribute = open.GetAttributeName(i);
                            string value = open.GetAttribute(attribute);
                            Console.Write(attribute + "=" + value + " ");
                        }
                        Console.WriteLine("]");
                        break;
                    case NodeType.Close:
                        CloseNode close = (CloseNode)node;
                        Console.WriteLine("Close: " + close.Name);
                        break;
                    case NodeType.Content:
                        Console.WriteLine("Content: " + node.Raw);
                        break;
                }
            }

            Console.WriteLine();
            foreach (Node node in n)
            {
                Console.Write(node.Raw);
            }
            Console.WriteLine();
        }
    }
}
