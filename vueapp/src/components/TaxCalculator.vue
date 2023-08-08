<template>

    <div v-if="loading" class="loading">
        Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
    </div>

    <div class="container">
        <div class="row" style="padding:10px">
            <div class="form-group" style="border:groove">

                <!-- results -->
                <div id="results" class="pt-4">
                    <h2> Tax Criteria</h2>
                    <div class="form-group" style="padding:10px">
                        <div class="input-group" id="divAge">
                            <div class="input-group-prepend">
                                <div class="label label-default" style="padding-right:20px">What is your Age?</div>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="inlineCheckbox" @change="setRebate('Primary')" checked="checked">
                                <label class="form-check-label" for="inlineCheckbox1">Less than 65</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="inlineCheckbox" @change="setRebate('Secondary')" >
                                <label class="form-check-label" for="inlineCheckbox2">Between 65 and 75</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="inlineCheckbox" @change="setRebate('Tertiary')" >
                                <label class="form-check-label" for="inlineCheckbox3">Older than 75</label>
                            </div>
                   
                        </div>
                    </div>

                    <div class="form-group" style="padding:10px">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <div class="label label-default" style="padding-right:20px">Select your Salary Frequency?</div>
                            </div>
                            <div style="width:15%">
                                <select class="form-select form-select-lg mb-3" aria-label=".form-select-lg example" v-model="payFrequency" @change="reset()">
                                    <option value="Monthly" selected >Monthly</option>
                                    <option value="Annual">Annual</option>
                                </select>
                            </div>
                   
                        </div>
                    </div>

                    <div class="form-group" style="padding:10px">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <div class="label label-default" style="padding-right:30px">Gross Salary per Frequency?</div>
                            </div>
                            <div style="width:15%">
                                <label class="form-check-label" for="salary" style="align-items:flex-start">ZAR</label>
                                <input v-model="grossSalary" id="salary" type="number" class="form-control" min="0" oninput="validity.valid||(value='0');" placeholder="0">
                            </div>
                   
                        </div>
                    </div>

                    <div class="col-6" style="padding:10px">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <div class="label label-default" style="padding-right:30px">Calculate your Tax.</div>
                            </div>
                            <div style="width:10%">
                                <button class="btn btn-primary btn-lg" type="submit" @click="calculateNetSalary();">Calculate</button>
                            </div>
                        </div>
                    </div>
                    <div v-if="calculate" class ="col-6" style="padding:10px">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <div class="label label-default" style="padding-right:20px">Reset your Tax.</div>
                            </div>
                            <div style="width:30%" >
                                <button class="btn btn-primary btn-lg" type="submit" @click="reset();">Reset Tax</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div  v-if="calculate" class="row" style="padding:10px">
            <div class="form-group" style="border:double">

                <!-- results -->
                <div id="results" class="pt-4">
                    <h2> Calculation Result</h2>
                    <div class="form-group" style="padding:10px">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <h5>{{payFrequency}} PAYE Amount is:  {{payeAmount}} </h5>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" style="padding:10px">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <h3>Your {{payFrequency}} NET Salary is:  {{netSalary}} </h3>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div v-if="taxtable" class="content">
            <table class="table table-dark" width="50%" border="1">
                <thead>
                    <tr>
                        <th width="30%"></th>
                        <th width="70%"></th>
                    </tr>
                    <tr>
                        <th colspan="2">Income tax table for individuals</th>
                    </tr>
                    <tr>
                        <th colspan="2">2024 tax year (1 March 2023 - 29 February 2024)</th>
                    </tr>
                    <tr>
                        <th>Taxable income (R)</th>
                        <th align="center">Rates of tax (R)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="bracket in taxtable" :key="bracket.id">
                        <td>{{bracket.taxBracketStart + '  -  ' + bracket.taxBracketEnd}}</td>
                        <td>{{bracket.taxPeriodDescription}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                loading: false,
                rdPrimeChecked: false,
                calculate: false,
                taxtable: [],
                ageGroup: 0,
                grossSalary: 0,
                grossAnnual: 0,
                netSalary: '',
                payFrequency: 'Monthly',
                taxAbove: 0,
                percentage: 0,
                taxBracket: [],
                taxRebates: null,
                rebate: 0,
                threshHold: 0,
                payeAmount: '',
                baseTax: 0,
                totalTax: 0,
                salaryFormat: ''
            };
        },
        created() {

            this.salaryFormat = Intl.NumberFormat("en-ZA", {
                style: "currency",
                currency: "ZAR",
            }); 

            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();         
        },      
        watch: {
            '$route': 'fetchData',
             loading() {
                this.loading == false ? this.setRebate('Primary') : null;
             },
        },
        methods: {
            fetchData() {
                this.taxtable = null;
                this.taxRebates = null;
                this.loading = true;

                fetch('https://localhost:7195/api/TaxTables')
                    .then(r => r.json())
                    .then(json => {
                        console.log(json);
                        this.taxtable = json;
                        return;
                    });

                fetch('https://localhost:7195/api/TaxRebates/TaxRebatesByYear/2024')
                    .then(r => r.json())
                    .then(json => {
                        console.log(json);
                        this.taxRebates = json;    
                        this.loading = false;
                        return;
                    });
               
            },
            setTaxRange(){
            
                this.taxBracket = this.taxtable.find((item) => item.taxBracketEnd >= this.grossAnnual); 

                this.baseTax = this.taxBracket.taxBaseAmount;
                this.percentage = this.taxBracket.taxBasePercent;

                this.taxAbove = this.grossAnnual - this.taxBracket.taxBracketStart;

            },
            calculateTax() {

                let taxableAmount = 0;
                let netSalaryAmt = this.grossSalary;
                
                if (this.grossAnnual > this.threshHold) {

                    taxableAmount = this.taxAbove * this.percentage;
                    taxableAmount = (taxableAmount + this.baseTax);
                    taxableAmount = taxableAmount - this.rebate;

                    if (this.payFrequency != "Annual")
                        taxableAmount = taxableAmount / 12;

                    netSalaryAmt = this.grossSalary - taxableAmount; 
                }
                //console.log("% taxAmount " + taxableAmount);

                this.payeAmount = this.salaryFormat.format(taxableAmount);
                this.netSalary = this.salaryFormat.format(netSalaryAmt);
               
            },
            calculateNetSalary() {
                if (this.payFrequency == "Annual")
                    this.grossAnnual = this.grossSalary;
                else
                    this.grossAnnual = this.grossSalary * 12;


                this.setTaxRange();
                this.calculateTax();

                this.calculate = true;
            },
            setRebate(typeSelected) {

                if (this.taxRebates != null) {

                    switch (typeSelected) {

                        case 'Primary':

                            this.rebate = this.taxRebates[0].rebateAmount;
                            this.threshHold = this.taxRebates[0].threshHoldAmount;
                            break;

                        case 'Secondary':

                            this.rebate = this.taxRebates[0].rebateAmount + this.taxRebates[1].rebateAmount;
                            this.threshHold = this.taxRebates[1].threshHoldAmount;
                            break;

                        case 'Tertiary':

                            this.rebate = 0;
                            this.taxRebates.forEach(x => this.rebate += x.rebateAmount);
                            this.threshHold = this.taxRebates[2].threshHoldAmount;
                            break;

                        default: break;
                    }

                    //console.log("this.rebate: " + this.rebate);
                }
            },
            reset() {

                this.calculate = false;
                this.grossSalary = 0;
                this.grossAnnual = 0;
                this.netSalary = '';
                this.taxAbove = 0;
                this.taxAbove = 0;
                this.percentage = 0;
                this.payeAmount = '';
                this.baseTax = 0;
                this.totalTax = 0;
            },
        },
    });
</script>