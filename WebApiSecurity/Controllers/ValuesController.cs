using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using WebApiSecurity.Services;

namespace WebApiSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly HashService _hashService;

        public ValuesController(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            _protector = protectionProvider.CreateProtector("Valor_para_Encriptacion");
            _hashService = hashService;
        }
        // GET api/values
        [HttpGet]
        // [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Test Hash with reandom Sal 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Hash")]
        public ActionResult GetHash()
        {
            string textoplano = "douglasLoaiza ";
            var hashResult = _hashService.Hash(textoplano).Hash;
            var hashResult2 = _hashService.Hash(textoplano).Hash;

            return Ok(new { textoplano, hashResult, hashResult2 });

        }



        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var protectorlimitedbytime = _protector.ToTimeLimitedDataProtector();

            string textoplano = "douglasLoaiza ";
            //Encript the textoplano
            //string textocifrado = _protector.Protect(textoplano);
            //encript by time
            string textocifrado = protectorlimitedbytime.Protect(textoplano,TimeSpan.FromSeconds(5));
            //Encript textoplano
            //string textodesncriptado = _protector.Unprotect(textocifrado);
            //Encript by time
            Thread.Sleep(6000);
            string textodesncriptado = protectorlimitedbytime.Unprotect(textocifrado);
            return Ok(new { textoplano, textocifrado, textodesncriptado });

            
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
