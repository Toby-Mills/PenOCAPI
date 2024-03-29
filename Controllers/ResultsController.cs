﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Web.Http.Cors;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResultsController : ApiController
    {
        //---------------------------------------------------------------------------------
        private static IQueryable<Result> QueryResults()
        {
            PenocEntities db = new PenocEntities();

            return (from result in db.tblResult
                    orderby result.intPosition
                    select new Result
                    {
                        eventId = result.tblCourse.intEvent,
                        eventName = result.tblCourse.tblEvent.strName,
                        eventDate = result.tblCourse.tblEvent.dteDate,
                        courseId = result.intCourse,
                        courseName = result.tblCourse.strName,
                        competitorId = result.intCompetitor,
                        competitor = result.tblCompetitor.strReadOnlyFullName,
                        categoryId = result.intCategory,
                        clubId = result.intClub,
                        comment = result.strComment,
                        disqualified = result.blnDisqualified,
                        points = result.intPoints,
                        position = result.intPosition,
                        raceNumber = result.strRaceNumber,
                        time = result.dteTime
                    });


        }

        //---------------------------------------------------------------------------------
        [HttpGet]
        [Route("courses/{idCourse}/results")]
        public IHttpActionResult GetCourseResults(int idCourse)
        {
            IQueryable<Result> queryResults = QueryResults().Where(r => r.courseId == idCourse);

            return Ok(queryResults);
        }

        //---------------------------------------------------------------------------------
        [HttpDelete]
        [Authorize]
        [JwtAuthentication]
        [Route("courses/{idCourse}/results")]
        public IHttpActionResult DeleteCourseResults(int idCourse)
        {
            PenocEntities db = new PenocEntities();

            IQueryable<tblResult> queryResults = db.tblResult.Where(r => r.intCourse == idCourse);

            db.tblResult.RemoveRange(queryResults);

            // Ask the DataContext to save all the changes.
            db.SaveChanges();
            return Ok();
        }

        //---------------------------------------------------------------------------------
        [HttpGet]
        [Route("courses/{idCourse}/results/{idCompetitor}")]
        public IHttpActionResult GetResult(int idCourse, int idCompetitor)
        {
            IQueryable<Result> queryResults = QueryResults().Where(r => r.courseId == idCourse && r.competitorId == idCompetitor);

            return Ok(queryResults.First());
        }

        //---------------------------------------------------------------------------------
        [HttpGet]
        [Route("competitors/{idCompetitor}/results")]
        public IHttpActionResult GetCompetitorResult(int idCompetitor)
        {
            IQueryable<Result> queryResults = QueryResults().Where(r =>  r.competitorId == idCompetitor);

            return Ok(queryResults);
        }


        //---------------------------------------------------------------------------------
        [HttpPut]
        [Authorize]
        [JwtAuthentication]
        [Route("results")]
        public IHttpActionResult UpdateResult(Result result)
        {
            PenocEntities db = new PenocEntities();

            tblResult resultRecord = db.tblResult.Single(r => r.intCourse == result.courseId && r.intCompetitor == result.competitorId);

            resultRecord.intCategory = result.categoryId;
            resultRecord.intClub = result.clubId;
            resultRecord.intPoints = result.points;
            resultRecord.intPosition = result.position;
            resultRecord.strComment = result.comment;
            resultRecord.strRaceNumber = result.raceNumber;
            resultRecord.blnDisqualified = result.disqualified;
            resultRecord.dteTime = result.time;

            db.SaveChanges();

            return Ok(result);
        }

        //---------------------------------------------------------------------------------
        [HttpPut]
        [Authorize]
        [JwtAuthentication]
        [Route("courses/{courseId}/results")]
        public IHttpActionResult UpdateResult(int courseId, Result[] courseResults)
        {

            PenocEntities db = new PenocEntities();

            IQueryable<tblResult> queryResults = db.tblResult.Where(r => r.intCourse == courseId);
            db.tblResult.RemoveRange(queryResults);

            foreach (Result courseResult in courseResults) {
                tblResult resultRecord = new tblResult
                {
                    intCourse = courseId,
                    intPosition = courseResult.position,
                    intCompetitor = courseResult.competitorId,
                    intClub = courseResult.clubId,
                    intCategory = courseResult.categoryId,
                    strRaceNumber = courseResult.raceNumber,
                    dteTime = courseResult.time,
                    intPoints = courseResult.points,
                    blnDisqualified = courseResult.disqualified,
                    strComment = courseResult.comment
                };
                db.tblResult.Add(resultRecord);
            };

            db.SaveChanges();

            return Ok();
        }

    }

}
