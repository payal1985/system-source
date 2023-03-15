using CDN_test.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CDN_test.Models.Aws_Models;
using System.Collections.Generic;
using AutoMapper;

namespace CDN_test.Controllers
{
    [Route("~/s3")]
    public class S3Controller : Controller
    {
        private readonly IMapper _mapper;
        public S3Controller(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("~/s3/index")]
        public async Task<IActionResult> Index()
        {
            CDN_Repo cdnrepo = new CDN_Repo("private", true);
            var s3files = await cdnrepo.GetFiles();
            var s3filesDto = _mapper.Map<IEnumerable<S3File>>(s3files);
            return View(s3filesDto);
        }
    }
}
