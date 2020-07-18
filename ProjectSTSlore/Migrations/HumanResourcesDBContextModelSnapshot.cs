﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectSTSlore;

namespace ProjectSTSlore.Migrations
{
    [DbContext(typeof(HumanResourcesDBContext))]
    partial class HumanResourcesDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("ProjectSTSlore.Group", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("groupNumber")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("image")
                        .HasColumnType("blob");

                    b.HasKey("id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ProjectSTSlore.Group_TeacherSubject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("groupid")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("hours")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("teacherSubjectid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("groupid");

                    b.HasIndex("teacherSubjectid");

                    b.ToTable("Group_TeacherSubject");
                });

            modelBuilder.Entity("ProjectSTSlore.Marks", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("studentid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("subjectForMarksid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("studentid");

                    b.HasIndex("subjectForMarksid");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("ProjectSTSlore.Person", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("address")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<string>("patronymic")
                        .HasColumnType("TEXT");

                    b.Property<int>("personRole")
                        .HasColumnType("INTEGER");

                    b.Property<string>("surname")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("ProjectSTSlore.Student", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("groupid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("personid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("groupid");

                    b.HasIndex("personid");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("ProjectSTSlore.Subject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("subjectName")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("ProjectSTSlore.Teacher", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("personid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("personid");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("ProjectSTSlore.Teacher_Subject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("subjectid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("teacherid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("subjectid");

                    b.HasIndex("teacherid");

                    b.ToTable("Teacher_Subject");
                });

            modelBuilder.Entity("ProjectSTSlore.Group_TeacherSubject", b =>
                {
                    b.HasOne("ProjectSTSlore.Group", "group")
                        .WithMany()
                        .HasForeignKey("groupid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectSTSlore.Teacher_Subject", "teacherSubject")
                        .WithMany()
                        .HasForeignKey("teacherSubjectid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectSTSlore.Marks", b =>
                {
                    b.HasOne("ProjectSTSlore.Student", "student")
                        .WithMany()
                        .HasForeignKey("studentid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectSTSlore.Group_TeacherSubject", "subjectForMarks")
                        .WithMany()
                        .HasForeignKey("subjectForMarksid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectSTSlore.Student", b =>
                {
                    b.HasOne("ProjectSTSlore.Group", "group")
                        .WithMany()
                        .HasForeignKey("groupid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectSTSlore.Person", "person")
                        .WithMany()
                        .HasForeignKey("personid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectSTSlore.Teacher", b =>
                {
                    b.HasOne("ProjectSTSlore.Person", "person")
                        .WithMany()
                        .HasForeignKey("personid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectSTSlore.Teacher_Subject", b =>
                {
                    b.HasOne("ProjectSTSlore.Subject", "subject")
                        .WithMany()
                        .HasForeignKey("subjectid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectSTSlore.Teacher", "teacher")
                        .WithMany()
                        .HasForeignKey("teacherid")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
