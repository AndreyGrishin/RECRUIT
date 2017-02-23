using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RecruIT.Model.DBModel;

namespace RecruIT.Migrations
{
    [DbContext(typeof(HrContext))]
    partial class HrContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("RecruIT.Model.DBModel.ContactInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("Home");

                    b.Property<string>("LinkedIn");

                    b.Property<string>("Phone");

                    b.Property<string>("Skype");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("ContectInfo");
                });

            modelBuilder.Entity("RecruIT.Model.DBModel.Employees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<int?>("ContactInfoId");

                    b.Property<int>("ContactsInfoId");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Post");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ContactInfoId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("RecruIT.Model.DBModel.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RecruIT.Model.DBModel.Employees", b =>
                {
                    b.HasOne("RecruIT.Model.DBModel.ContactInfo", "ContactInfo")
                        .WithMany("Employees")
                        .HasForeignKey("ContactInfoId");
                });
        }
    }
}
