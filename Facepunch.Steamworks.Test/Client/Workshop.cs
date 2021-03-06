﻿using System;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Facepunch.Steamworks.Test
{
    [TestClass]
    [DeploymentItem( Config.LibraryName + ".dll" )]
    [DeploymentItem( "steam_appid.txt" )]
    public class WorkshopTest
    {
        [TestMethod]
        public void Query()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                var Query = client.Workshop.CreateQuery();

                Query.Run();

                // Block, wait for result
                // (don't do this in realtime)
                Query.Block();

                Assert.IsFalse( Query.IsRunning );
                Assert.IsTrue( Query.TotalResults > 0 );
                Assert.IsTrue( Query.Items.Length > 0 );

                Console.WriteLine( "Query.TotalResults: {0}", Query.TotalResults );
                Console.WriteLine( "Query.Items.Length: {0}", Query.Items.Length );

                // results

                Console.WriteLine( "Searching" );

                Query.Order = Workshop.Order.RankedByTextSearch;
                Query.QueryType = Workshop.QueryType.MicrotransactionItems;
                Query.SearchText = "shit";
                Query.RequireTags.Add( "LongTShirt Skin" );
                Query.Run();

                // Block, wait for result
                // (don't do this in realtime)
                Query.Block();

                Console.WriteLine( "Query.TotalResults: {0}", Query.TotalResults );
                Console.WriteLine( "Query.Items.Length: {0}", Query.Items.Length );

                Assert.IsTrue( Query.TotalResults > 0 );
                Assert.IsTrue( Query.Items.Length > 0 );

                foreach ( var item in Query.Items )
                {
                    Console.WriteLine( "{0}", item.Title );
                }
            }
        }

        [TestMethod]
        public void QueryTagRequire()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                using ( var Query = client.Workshop.CreateQuery() )
                {
                    Query.RequireTags.Add( "LongTShirt Skin" );
                    Query.Run();

                    Query.Block();

                    Assert.IsFalse( Query.IsRunning );
                    Assert.IsTrue( Query.TotalResults > 0 );
                    Assert.IsTrue( Query.Items.Length > 0 );

                    Console.WriteLine( "Query.TotalResults: {0}", Query.TotalResults );
                    Console.WriteLine( "Query.Items.Length: {0}", Query.Items.Length );

                    Assert.IsTrue( Query.TotalResults > 0 );
                    Assert.IsTrue( Query.Items.Length > 0 );

                    foreach ( var item in Query.Items )
                    {
                        Console.WriteLine( "{0}", item.Title );
                        Console.WriteLine( "\t{0}", string.Join( ";", item.Tags ) );

                        Assert.IsTrue( item.Tags.Contains( "LongTShirt Skin" ) );
                    }
                }
            }
        }

        [TestMethod]
        public void QueryTagExclude()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                using ( var Query = client.Workshop.CreateQuery() )
                {
                    Query.RequireTags.Add( "LongTShirt Skin" );
                    Query.ExcludeTags.Add( "version2" );
                    Query.Run();

                    Query.Block();

                    Assert.IsFalse( Query.IsRunning );
                    Assert.IsTrue( Query.TotalResults > 0 );
                    Assert.IsTrue( Query.Items.Length > 0 );

                    Console.WriteLine( "Query.TotalResults: {0}", Query.TotalResults );
                    Console.WriteLine( "Query.Items.Length: {0}", Query.Items.Length );

                    Assert.IsTrue( Query.TotalResults > 0 );
                    Assert.IsTrue( Query.Items.Length > 0 );

                    foreach ( var item in Query.Items )
                    {
                        Console.WriteLine( "{0}", item.Title );
                        Console.WriteLine( "\t{0}", string.Join( ";", item.Tags ) );

                        Assert.IsTrue( item.Tags.Contains( "LongTShirt Skin" ) );
                        Assert.IsFalse( item.Tags.Contains( "version2" ) );
                    }
                }
            }
        }

        [TestMethod]
        public void QueryFile()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                using ( var Query = client.Workshop.CreateQuery() )
                {
                    Query.FileId.Add( 751993251 );
                    Query.Run();

                    Assert.IsTrue( Query.IsRunning );

                    Query.Block();

                    Assert.IsFalse( Query.IsRunning );
                    Assert.AreEqual( Query.TotalResults, 1 );
                    Assert.AreEqual( Query.Items.Length, 1 );

                    Console.WriteLine( "Query.TotalResults: {0}", Query.TotalResults );
                    Console.WriteLine( "Query.Items.Length: {0}", Query.Items.Length );

                    Assert.AreEqual<ulong>( Query.Items[0].Id, 751993251 );
                }
            }
        }

        [TestMethod]
        public void QueryFiles()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                using ( var Query = client.Workshop.CreateQuery() )
                {
                    Query.FileId.Add( 751993251 );
                    Query.FileId.Add( 747266909 );
                    Query.Run();

                    Assert.IsTrue( Query.IsRunning );

                    Query.Block();

                    Assert.IsFalse( Query.IsRunning );
                    Assert.AreEqual( Query.TotalResults, 2 );
                    Assert.AreEqual( Query.Items.Length, 2 );

                    Console.WriteLine( "Query.TotalResults: {0}", Query.TotalResults );
                    Console.WriteLine( "Query.Items.Length: {0}", Query.Items.Length );

                    Assert.IsTrue( Query.Items.Any( x => x.Id == 751993251 ) );
                    Assert.IsTrue( Query.Items.Any( x => x.Id == 747266909 ) );
                }
            }
        }

        [TestMethod]
        public void DownloadFile()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                using ( var Query = client.Workshop.CreateQuery() )
                {
                    Query.FileId.Add( 661319648 );
                    Query.Run();

                    Assert.IsTrue( Query.IsRunning );

                    Query.Block();

                    Assert.IsFalse( Query.IsRunning );
                    Assert.AreEqual( Query.TotalResults, 1 );
                    Assert.AreEqual( Query.Items.Length, 1 );

                    var item = Query.Items[0];

                    if ( !item.Installed )
                    {
                        item.Download();

                        while ( item.Downloading )
                        {
                            Thread.Sleep( 500 );
                            client.Update();
                            Console.WriteLine( "Download Progress: {0}", item.DownloadProgress );
                        }
                    }

                    Assert.IsNotNull( item.Directory );
                    Assert.AreNotEqual( 0, item.Size );

                    Console.WriteLine( "item.Installed:         {0}", item.Installed );
                    Console.WriteLine( "item.Downloading:       {0}", item.Downloading );
                    Console.WriteLine( "item.DownloadPending:   {0}", item.DownloadPending );
                    Console.WriteLine( "item.Directory:         {0}", item.Directory );
                    Console.WriteLine( "item.Size:              {0}mb", (item.Size / 1024 / 1024) );

                }
            }
        }

        [TestMethod]
        [TestCategory( "Run Manually" )]
        public void CreatePublish()
        {
            using ( var client = new Facepunch.Steamworks.Client( 252490 ) )
            {
                Assert.IsTrue( client.IsValid );

                var item = client.Workshop.CreateItem( Workshop.ItemType.Microtransaction );

                item.Title = "Facepunch.Steamworks Unit test";

                item.Publish();

                while ( item.Publishing )
                {
                    client.Update();
                    Thread.Sleep( 100 );
                }

                Assert.IsFalse( item.Publishing );
                Assert.AreNotEqual( 0, item.Id );

                Console.WriteLine( "item.Id: {0}", item.Id );

                item.Delete();
            }
        }

    }
}
