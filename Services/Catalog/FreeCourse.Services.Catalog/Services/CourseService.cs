﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
	public class CourseService :ICourseService
	{
		private readonly IMongoCollection<Course> _courseCollection;
		private readonly IMongoCollection<Category> _categoryCollection;

		private readonly IMapper _mapper;

		private readonly IPublishEndpoint _publishEndpoint;

		public CourseService(IMapper mapper, IDatabaseSettings databaseSettings,IPublishEndpoint publishEndpoint)
		{
			var client = new MongoClient(databaseSettings.ConnectionString);

			var database = client.GetDatabase(databaseSettings.DatabaseName);

			_categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
			_courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
			_mapper = mapper;
			_publishEndpoint = publishEndpoint;
		}

		public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllAsync()
		{
			var courses = await _courseCollection.Find(c => true).ToListAsync();

			if(courses.Any())
            {
                foreach (var course in courses)
                {
					course.Category = await _categoryCollection.Find<Category>(c => c.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
				courses = new List<Course>();
            }

			return Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
		}

		public async Task<Shared.Dtos.Response<CourseDto>> GetByIdAsync(string id)
        {
			var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

			if(course == null)
            {
				return Shared.Dtos.Response<CourseDto>.Fail("Course not found",404);
            }
			course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();

			return Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

		public async Task <Shared.Dtos.Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
			var courses = await _courseCollection.Find<Course>(c => c.UserID == userId).ToListAsync();

			if (courses.Any())
			{
				foreach (var course in courses)
				{
					course.Category = await _categoryCollection.Find<Category>(c => c.Id == course.CategoryId).FirstAsync();
				}
			}
			else
			{
				courses = new List<Course>();
			}
			return Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
		}

		public async Task<Shared.Dtos.Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
			var newCourse = _mapper.Map<Course>(courseCreateDto);
			newCourse.CreatedTime = DateTime.Now;
			await _courseCollection.InsertOneAsync(newCourse);

			return Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

		public async Task<Shared.Dtos.Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
			var updateCourse = _mapper.Map<Course>(courseUpdateDto);

			var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

			if(result == null)
            {
				return Shared.Dtos.Response<NoContent>.Fail("Course not found", 404);
            }

			await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent
			{
				CourseId = updateCourse.Id,
				UpdatedName = updateCourse.Name
			});
			return Shared.Dtos.Response<NoContent>.Success(204);
        }

        public async Task<Shared.Dtos.Response<NoContent>> DeleteAsync(string id)
        {
			var result = await _courseCollection.DeleteOneAsync(c => c.Id == id);

			if(result.DeletedCount >0)
            {
				return Shared.Dtos.Response<NoContent>.Success(204);
            }
            else
            {
				return Shared.Dtos.Response<NoContent>.Fail("Course not fount", 404);
            }
        }

    }
}