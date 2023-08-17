using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.Controllers
{
    public class ExpertTaskController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public ExpertTaskController(NewIspanProjectContext context)
        {
            _context = context;
        }
        #region 歷史委託
        // GET: ExpertTask
        public async Task<IActionResult> ExpertList() //歷史委託
        {
            var newIspanProjectContext = _context.TaskLists.Include(t => t.Payment).Include(t => t.PaymentDate).Include(t => t.Salary).Include(t => t.Town);
            return View(await newIspanProjectContext.ToListAsync());
        }
        #endregion

        // GET: ExpertTask/Details/5
        public async Task<IActionResult> DetailsReceive(int? id)
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();            }

            var taskList = await _context.TaskLists
                .Include(t => t.Payment)
                .Include(t => t.PaymentDate)
                .Include(t => t.Salary)
                .Include(t => t.Town)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (taskList == null)
            {
                //return NotFound();
                return View();            }

            return View(taskList);
        }

# region 收到委託
        public async Task<IActionResult> DetailsResponse(int? id) //收到委託
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();            }

            var taskList = await _context.TaskLists
                .Include(t => t.Payment)
                .Include(t => t.PaymentDate)
                .Include(t => t.Salary)
                .Include(t => t.Town)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (taskList == null)
            {
                //return NotFound();
                return View();
            }

            return View(taskList);
        }
#endregion

        #region 檢視
        public async Task<IActionResult> DetailsView(int? id)//檢視
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();
            }

            var taskList = await _context.TaskLists
                .Include(t => t.Payment)
                .Include(t => t.PaymentDate)
                .Include(t => t.Salary)
                .Include(t => t.Town)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (taskList == null)
            {
                //return NotFound();
                return View();
            }

            return View(taskList);
        }

        #endregion

        #region 從履歷連委託單
        // GET: ExpertTask/Create
        public IActionResult Create()  //從履歷連委託單
        {
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId");
            ViewData["PaymentDateId"] = new SelectList(_context.PaymentDates, "PaymentDateId", "PaymentDateId");
            ViewData["SalaryId"] = new SelectList(_context.Salaries, "SalaryId", "SalaryId");
            ViewData["TownId"] = new SelectList(_context.Towns, "TownId", "TownId");
            return View();
        }

        // POST: ExpertTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  //從履歷連委託單
        public async Task<IActionResult> Create([Bind("CaseId,AccountId,TaskNameId,TaskTitle,TaskDetail,WorkingHoursId,PayFrom,PayTo,PaymentId,PaymentDateId,SalaryId,TaskPlace,TownId,WorkPlace,Address,RequiredNum,TaskPeriod,TaskStartHour,TaskEndHour,TaskStartDate,TaskEndDate,Requirement,HumanList,LanguageRequired,ServiceStatus,StatusChangeReasonId,PublishOrNot,PublishStart,PublishEnd,CaseStatusId,OnTop,DataCreateDate,DataModifyDate,DataModifyPerson,IsExpert")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ExpertList));
            }
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", taskList.PaymentId);
            ViewData["PaymentDateId"] = new SelectList(_context.PaymentDates, "PaymentDateId", "PaymentDateId", taskList.PaymentDateId);
            ViewData["SalaryId"] = new SelectList(_context.Salaries, "SalaryId", "SalaryId", taskList.SalaryId);
            ViewData["TownId"] = new SelectList(_context.Towns, "TownId", "TownId", taskList.TownId);
            return View(taskList);
        }

        #endregion
        #region 修改委託單
        // GET: ExpertTask/Edit/5
        public async Task<IActionResult> Edit(int? id) //修改委託單
        {
            if (id == null || _context.TaskLists == null)
            {
                //return NotFound();
                return View();
            }

            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList == null)
            {
                //return NotFound();
                return View();
            }
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", taskList.PaymentId);
            ViewData["PaymentDateId"] = new SelectList(_context.PaymentDates, "PaymentDateId", "PaymentDateId", taskList.PaymentDateId);
            ViewData["SalaryId"] = new SelectList(_context.Salaries, "SalaryId", "SalaryId", taskList.SalaryId);
            ViewData["TownId"] = new SelectList(_context.Towns, "TownId", "TownId", taskList.TownId);
            return View(taskList);
        }

        // POST: ExpertTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  //修改委託單
        public async Task<IActionResult> Edit(int id, [Bind("CaseId,AccountId,TaskNameId,TaskTitle,TaskDetail,WorkingHoursId,PayFrom,PayTo,PaymentId,PaymentDateId,SalaryId,TaskPlace,TownId,WorkPlace,Address,RequiredNum,TaskPeriod,TaskStartHour,TaskEndHour,TaskStartDate,TaskEndDate,Requirement,HumanList,LanguageRequired,ServiceStatus,StatusChangeReasonId,PublishOrNot,PublishStart,PublishEnd,CaseStatusId,OnTop,DataCreateDate,DataModifyDate,DataModifyPerson,IsExpert")] TaskList taskList)
        {
            if (id != taskList.CaseId)
            {
                //return NotFound();
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskListExists(taskList.CaseId))
                    {
                        //return NotFound();
                        return View();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ExpertList));
            }
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", taskList.PaymentId);
            ViewData["PaymentDateId"] = new SelectList(_context.PaymentDates, "PaymentDateId", "PaymentDateId", taskList.PaymentDateId);
            ViewData["SalaryId"] = new SelectList(_context.Salaries, "SalaryId", "SalaryId", taskList.SalaryId);
            ViewData["TownId"] = new SelectList(_context.Towns, "TownId", "TownId", taskList.TownId);
            return View(taskList);
        }
        #endregion





        //-------------------------------------------------
        private bool TaskListExists(int id)
        {
          return (_context.TaskLists?.Any(e => e.CaseId == id)).GetValueOrDefault();
        }
    }
}
