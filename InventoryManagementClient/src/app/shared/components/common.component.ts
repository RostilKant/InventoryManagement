import {FormGroup} from "@angular/forms";

export class CommonComponent {
  static getErrorMessage(form: FormGroup, ...controlNames: string[]): string {
    let spacesBetween = controlNames.map(x => `${x.toLowerCase()} `)
    spacesBetween[0] = this.toTitleCase(spacesBetween[0])

    if (form.get(controlNames.join(''))?.errors?.required) {
      return `${spacesBetween.join(' ')} is required`
    }

    if (form.get(controlNames)?.errors?.minlength) {
      return `${spacesBetween} must contain at least
      ${form.get(controlNames)?.errors?.minlength.requiredLength} symbols`
    }

    if (form.get(controlNames)?.errors?.email) {
      return `${spacesBetween} is invalid`
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
}
