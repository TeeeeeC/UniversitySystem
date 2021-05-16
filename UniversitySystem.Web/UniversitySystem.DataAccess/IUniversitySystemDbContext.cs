﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using UniversitySystem.DataAccess.Models;

namespace UniversitySystem.DataAccess
{
    public interface IUniversitySystemDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry Entry([NotNullAttribute] object entity);

        int SaveChanges();

        DbSet<Student> Students { get; set; }

        DbSet<StudentCourse> StudentCourses { get; set; }

        DbSet<Course> Courses { get; set; }
    }
}
