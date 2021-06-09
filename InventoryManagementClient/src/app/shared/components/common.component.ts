import {FormGroup} from "@angular/forms";
import {EmployeeDepartment} from "../enums/employee-department.enum";

export class CommonComponent {
  static getErrorMessage(form: FormGroup, ...controlNames: string[]): string {
    let spacesBetween = controlNames.map(x => `${x.toLowerCase()} `)
    spacesBetween[0] = this.toTitleCase(spacesBetween[0])

    if (form.get(controlNames[0])?.errors?.email) {
      return `${this.toTitleCase(controlNames[0])} is invalid`
    }

    if (form.get(controlNames.join('').trim())?.errors?.required) {
      return `${spacesBetween.join(' ').trim()} is required`
    }

    if (form.get(controlNames)?.errors?.minlength) {
      return `${spacesBetween} must contain at least
      ${form.get(controlNames)?.errors?.minlength.requiredLength} symbols`
    }

    return ''
  }

    static toTitleCase(str: string) {
    return str.replace(
      /\w\S*/g,
      function(txt: string) {
        return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
      }
    );
  }

  static getEnumNames(enm: Object): string[] {
    return Object.values(enm).filter(d => isNaN(+d))
  }
}
