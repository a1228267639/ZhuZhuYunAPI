﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZhuZhuYunAPI.Models;

#nullable disable

namespace ZhuZhuYunAPI.Migrations
{
    [DbContext(typeof(PanoUserContext))]
    partial class PanoUserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8_general_ci")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8");

            modelBuilder.Entity("ZhuZhuYunAPI.Models.BlackRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ip");

                    b.Property<string>("Location_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location_date");

                    b.Property<string>("Machine_Code")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("machine_code");

                    b.HasKey("Id");

                    b.ToTable("BlackRecord");
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.LoginData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("info");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("password");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("LoginData_Table", (string)null);
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.PanoDownLoadRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ip");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("Location_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location_date");

                    b.Property<string>("Machine_Code")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("machine_code");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("PanoDownLoadRecord_Table", (string)null);
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.PanoLoginRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ip");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("Location_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location_date");

                    b.Property<string>("Machine_Code")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("machine_code");

                    b.Property<int>("UserID")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("PanoLoginRecord_Table", (string)null);
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.PanoRegisterRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ip");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("Location_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location_date");

                    b.Property<string>("Machine_Code")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("machine_code");

                    b.Property<int>("UserID")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("PanoRegisterRecord");
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.PanoTempUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("End_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("end_date");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ip");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("Machine_Code")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("machine_code");

                    b.Property<string>("Reg_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("reg_date");

                    b.Property<int>("Reg_Day")
                        .HasColumnType("int")
                        .HasColumnName("reg_day");

                    b.Property<int>("UserID")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("PanoTempUser_Table", (string)null);
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.PanoUserRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("End_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("end_date");

                    b.Property<string>("Reg_Date")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("reg_date");

                    b.Property<int>("Reg_Day")
                        .HasColumnType("int")
                        .HasColumnName("reg_day");

                    b.Property<string>("Reg_Info")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("reg_info");

                    b.Property<string>("Reg_Money")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("reg_money");

                    b.Property<string>("Reg_Vocher")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("reg_vocher");

                    b.Property<int>("UserID")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("PanoUser_Table", (string)null);
                });

            modelBuilder.Entity("ZhuZhuYunAPI.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("BindMachine_Count")
                        .HasColumnType("int")
                        .HasColumnName("bind_machinecount");

                    b.Property<string>("Machine_Codes")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("machine_codes");

                    b.Property<int>("UserID")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int>("User_Type")
                        .HasColumnType("int")
                        .HasColumnName("user_type");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
